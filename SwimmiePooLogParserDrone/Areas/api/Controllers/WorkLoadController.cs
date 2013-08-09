using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using CommonCore;
using SwimmiePooLogParser.Common.DTOs;
using SwimmiePooLogParserDrone.Core.Repositories;
using SwimmiePooLogParserDrone.UI.Helpers.ModelBinders;

namespace SwimmiePooLogParserDrone.UI.Areas.api.Controllers
{
    public class WorkLoadController : BaseApiController
    {
        public IRequestRepository RequestRepository
        {
            get { return DependencyResolver.Get<IRequestRepository>(); }
        }

        private static bool _notQueuing = true;
        private static bool _notProcessing = true;
        public static ConcurrentQueue<WorkLoadItem> _workQueue = new ConcurrentQueue<WorkLoadItem>();
        public static ConcurrentQueue<WorkLoadItem> _inProgressQueue = new ConcurrentQueue<WorkLoadItem>();

        private static int _maxProcessingCount = 100;
        private static int _maxQueueCount = 200;

        [HttpGet]
        [ActionName("Test")]
        public string Test()
        {
            return "Works";
        }

        [HttpGet]
        [ActionName("GetAvailableQueueCount")]
        public int GetAvailableQueueCount()
        {
            return _maxQueueCount - _workQueue.Count;
        }

        [HttpGet]
        [ActionName("GetProcessingCount")]
        public int GetProcessingCount()
        {
            return _inProgressQueue.Count;
        }

        [HttpGet]
        [ActionName("GetQueuedCount")]
        public int GetQueuedCount()
        {
            return _workQueue.Count;
        }

        [HttpPost]
        [ActionName("SetMaxProcessingCount")]
        public bool SetMaxProcessingCount([FromBody]int maxProcessingCount)
        {
            _maxProcessingCount = maxProcessingCount;
            return true;
        }

        [HttpPost]
        [ActionName("SetMaxQueueCount")]
        public bool SetMaxQueueCount([FromBody]int maxQueueCount)
        {
            _maxQueueCount = maxQueueCount;
            return true;
        }

        [HttpPost]
        //[HttpGet]
        [ActionName("BeginProcessingLoad")]
        public async Task<int> BeginProcessingLoad(/*[FromUri(Name = "WorkLoad")]*/ [FromBody]WorkLoad workLoad)
        {
            int numQueued = 0;
            foreach (var workLoadLine in workLoad.WorkLines)
            {
                if (_workQueue.Count < _maxQueueCount)
                {
                    _workQueue.Enqueue(workLoadLine);
                    numQueued++;
                }
            }

            var queueCount = numQueued;

            InitQueuing();
            return queueCount;
        }

        public Task InitQueuing()
        {
            if (_notQueuing)
            {
                _notQueuing = false;
                var task = Task.Factory.StartNew(() =>
                    {
                        FillInProgress();
                        _notQueuing = true;
                    },
                                                 CancellationToken.None,
                                                 TaskCreationOptions.LongRunning,
                                                 TaskScheduler.Default);
                return task;
            }
            return null;
        }

        private Task InitProcessing()
        {
            if (_notProcessing)
            {
                _notProcessing = false;
                var task = Task.Factory.StartNew(() =>
                    {
                        ProcessReady();
                        _notProcessing = true;
                    },
                                                 CancellationToken.None,
                                                 TaskCreationOptions.LongRunning,
                                                 TaskScheduler.Default);
                return task;
            }
            return null;
        }

        private void ProcessReady()
        {
            WorkLoadItem workItem;
            while (_inProgressQueue.TryPeek(out workItem))
            {
                try
                {
                    var requestRepository = RequestRepository;
                    var parsed = ParseWorkItem(workItem);
                    if (parsed != null)
                    {
                        requestRepository.AddNewRequest(parsed.RequestDateTime, parsed.ServerIPAddress,
                                                        parsed.ClientIPAddress,
                                                        parsed.RequestLength, parsed.OriginatingLogFile,
                                                        parsed.OriginatingLogFilePath, parsed.FileLineNumber);
                    }
                }
                catch (Exception)
                {
                }
                
                //requestRepository.AddNewRequest();

                _inProgressQueue.TryDequeue(out workItem);
            }
        }

        private void FillInProgress()
        {
            //if (_inProgressQueue.Count < 10)
            //{
            while (_workQueue.Count > 0)
            {
                WorkLoadItem workItem;
                while (_inProgressQueue.Count < _maxProcessingCount && _workQueue.TryDequeue(out workItem))
                {
                    _inProgressQueue.Enqueue(workItem);
                }
                InitProcessing();
                Thread.Sleep(10);
            }

            //}
            //WorkLoadItem workItem;
            //while (_workQueue.TryDequeue(out workItem))
            //    Thread.Sleep(1000);
        }

        private static Regex m = new Regex(@"\s", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static dynamic ParseWorkItem(WorkLoadItem workLoadItem)
        {

            string[] fields = m.Split(workLoadItem.WorkLine); // THIS LINE DOES THE MAGIC
            if (fields.Length == 14)
            {
                return new
                    {
                        //					RequestDate = fields[0],
                        //					RequestTime = fields[1],
                        RequestDateTime = DateTime.Parse(fields[0] + " " + fields[1]),
                        ServerIPAddress = fields[2], //ips table
                        //Method = fields[3], //methods table
                        //UriStem = fields[4],
                        //UriQuery = fields[5],
                        //Port = int.Parse(fields[6]), //ports table
                        //UserName = fields[7], //Usernames
                        ClientIPAddress = fields[8], //ips table
                        //UserAgent = fields[9], //user agents
                        //Status = int.Parse(fields[10]), //statuses table
                        //SubStatus = fields[11],
                        //Win32Status = fields[12],
                        RequestLength = int.Parse(fields[13]),
                        OriginatingLogFile = workLoadItem.LogFile.Replace(workLoadItem.LogFilePath, ""),
                        OriginatingLogFilePath = workLoadItem.LogFilePath,
                        FileLineNumber = workLoadItem.LineNumber
                    };
            }
            return null;
        }
    }
}
