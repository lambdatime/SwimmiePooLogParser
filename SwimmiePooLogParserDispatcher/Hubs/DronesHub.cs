using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SwimmiePooLogParser.Common.DTOs;
using SwimmiePooLogParserDispatcher.Core;
using SwimmiePooLogParserDispatcher.Core.Repositories;
using SwimmiePooLogParserDispatcher.Helpers;
using SwimmiePooLogParserDispatcher.Models;

namespace SwimmiePooLogParserDispatcher.Hubs
{
    [HubName("Drones")]
    public class DronesHub : Hub
    {
        private IDroneRepository DroneRepository
        {
            get { return DependencyResolver.Current.GetService<IDroneRepository>(); }
        }
        private ICurrentParseLinesRepository CurrentParseLinesRepository
        {
            get { return DependencyResolver.Current.GetService<ICurrentParseLinesRepository>(); }
        }

        public bool Add(Drone newDrone)
        {
            try
            {
                newDrone.ModifiedOn = DateTime.Now;
                DroneRepository.AddDrone(newDrone);

                StartDispatching();
                Clients.All.droneAdded(newDrone);
                return true;
            }
            catch (Exception)
            {
                Clients.Caller.reportError("Unable to update drone.");
                return false;
            }
        }

        public bool Remove(int Id)
        {
            try
            {
                var drone = DroneRepository.Get(Id);
                if (drone != null)
                {
                    DroneRepository.DeleteDrone(drone);

                    StartDispatching();
                    Clients.All.droneRemoved(Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Error : " + ex.Message);
                return false;
            }      
            return false;
        }

        public bool EnableProcessing(int Id)
        {
            try
            {
                StartDispatching();
                Clients.All().droneProcessingEnabled(Id);
                return true;
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Error : " + ex.Message);
                return false;
            }
        }

        public bool Abort(int Id)
        {
            try
            {
                Clients.All().droneAborting(Id);
                return true;
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Error : " + ex.Message);
                return false;
            }
        }

        public void GetAll()
        {
            StartDispatching();
            var allDrones = DroneRepository.GetAll().ToList();
            var res = allDrones
                .Select(d => new DroneDisplay(d))
                .OrderBy(d => d.SortOrder);
            Clients.Caller.droneAll(res.ToArray());
        }

        private static bool _notDispatching = true;
        private Task StartDispatching()
        {
            var filesAreAvailable = GetNextAvailableFile() != null;
            var allDrones = DroneRepository.GetAll().ToList();
            if (filesAreAvailable && allDrones.Count > 0 && _notDispatching)
            {
                _notDispatching = false;
                var task = Task.Factory.StartNew(() =>
                {
                    var droneAvailability = GetDroneAvailability().ToList();
                    while (filesAreAvailable)
                    {
                        if (droneAvailability.Sum(x => x.AvailableSlots) > 0)
                        {
                            QueueUpForAllAvailability();
                        }
                        droneAvailability = GetDroneAvailability().ToList();
                        filesAreAvailable = GetNextAvailableFile() != null;
                        Thread.Sleep(10);
                    }
                    _notDispatching = true;
                },
                                                 CancellationToken.None,
                                                 TaskCreationOptions.LongRunning,
                                                 TaskScheduler.Default);
                return task;
            }
            return null;
        }

        private void QueueUpForAllAvailability()
        {
            var droneAvailability = GetDroneAvailability().ToList();
            foreach (var drone in droneAvailability)
            {
                int remainder = drone.AvailableSlots;
                while (remainder > 0)
                {
                    //is a file already parsed?
                    var readyToAssign = CurrentParseLinesRepository.GetTopParseLinesToAssign(drone.AvailableSlots).ToList();
                    if (!readyToAssign.Any())
                    {
                        //break out if there is nothing left to parse, otherwise this would never end potentially
                        if (GetNextAvailableFile() == null)
                            break;
                        var wasFileParsedBefore = CurrentParseLinesRepository.FileCompletelyParsed();
                        ParseNextFileToDb(wasFileParsedBefore);
                        readyToAssign = CurrentParseLinesRepository.GetTopParseLinesToAssign(drone.AvailableSlots).ToList();
                    }

                    remainder = drone.AvailableSlots - readyToAssign.Count;
                    //assign whatever we can
                    foreach (var parseLine in readyToAssign)
                    {
                        parseLine.DroneId = drone.DroneId;
                        CurrentParseLinesRepository.Update(parseLine);
                    }

                    var task = Task.Factory.StartNew(() =>
                    {
                        //send out the work load
                        var service = new DroneWorkloadService(drone.DroneUrl);
                        //var droneWorkLoadItems = CurrentParseLinesRepository.GetWorkLoadByDroneId(drone.DroneId).ToList();
                        var workLoad = new WorkLoad
                        {
                            WorkLines = readyToAssign.Select(x => new WorkLoadItem
                            {
                                WorkLine = x.Line,
                                LineNumber = x.LineNumber,
                                LogFilePath = x.Path,
                                LogFile = x.FileName
                            }).ToArray()
                        };
                        service.BeginProcessingLoad(workLoad);
                    });
                }


            }
        }

        private void ArchiveCurrentFile()
        {
            //const string archiveLogDir = @"D:\Dev\Home\SwimmiePooLogParser\SwimmiePooLogParserDispatcher\App_Data\ArchivedLogs\";
            var currentFile = GetNextAvailableFile();
            var fileName = currentFile.Name;

            var currentFileDirectory = currentFile.Directory.FullName;
            var difference = currentFileDirectory.Replace(_baseLogDir, "");

            var newDirectory = Path.Combine(_archiveLogDir, difference);
            var archiveFile = Path.Combine(newDirectory, fileName);
            if (!Directory.Exists(newDirectory))
                Directory.CreateDirectory(newDirectory);

            File.Move(currentFile.FullName, archiveFile);
        }
        

        private int ParseNextFileToDb(bool archive = true)
        {
            var currentFile = GetNextAvailableFile();
            var currentFileName = currentFile.Name;
            var currentFilePath = currentFile.DirectoryName;
            if (archive)
                ArchiveCurrentFile();
            //purge the current parsed
            CurrentParseLinesRepository.RemoveAll(currentFileName, currentFilePath);

            var nextFile = GetNextAvailableFile();
            var fileName = nextFile.Name;
            var parseableLines = GetParseableLines(nextFile.FullName).ToList();

            CurrentParseLinesRepository.AddRange(parseableLines.Select(x => new CurrentParseLine
            {
                FileName = x.LogFile,
                Path = x.LogFilePath,
                LineNumber = x.LineNumber,
                Line = x.WorkLine                
            }));
            return parseableLines.Count;
        }

        private IEnumerable<DroneAvailability> GetDroneAvailability()
        {
            var allDrones = DroneRepository.GetAll().ToList();
            foreach (var drone in allDrones)
            {
                int? availableSlots = null;
                try
                {
                    var service = new DroneWorkloadService(drone.BaseUrl);
                    availableSlots = service.GetAvailableQueueCount();
                   
                }
                catch (Exception)
                {
                    
                }
                if (availableSlots.HasValue)
                    yield return new DroneAvailability
                    {
                        DroneId = drone.DroneId,
                        DroneUrl = drone.BaseUrl,
                        AvailableSlots = availableSlots.Value
                    };
                
            }
        }

        private FileInfo GetNextAvailableFile()
        {
            //const string baseLogDir = @"D:\Dev\Home\SwimmiePooLogParser\SwimmiePooLogParserDispatcher\App_Data\Logs\";

            var dir = new DirectoryInfo(_baseLogDir);
            var files = dir.GetFiles("*.log", SearchOption.AllDirectories);
            if (files.Any())
                return files.First();

            return null;
        }

        static readonly Regex m = new Regex(@"\s", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly string _archiveLogDir = SwimmiePooLogParserDispatcher.Properties.Settings.Default.ArchiveLogDir;
        private static readonly string _baseLogDir = SwimmiePooLogParserDispatcher.Properties.Settings.Default.LogDir;

        static IEnumerable<WorkLoadItem> GetParseableLines(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            using (var rdr = File.OpenText(fileName))
            {
                string line;
                var lineNum = 0;
                while ((line = rdr.ReadLine()) != null)
                {
                    var fields = m.Split(line); // THIS LINE DOES THE MAGIC
                    if (fields.Length == 14)
                    {
                        yield return new WorkLoadItem
                        {
                            LineNumber = lineNum,
                            LogFile = fileInfo.Name,
                            LogFilePath = fileInfo.DirectoryName,
                            WorkLine = line
                        };
                    }
                    lineNum++;
                }
            }
        }
    }
}