using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwimmiePooLogParserDrone.Core.Repositories;

namespace SwimmiePooLogParserDrone.Infrastructure.Data.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        public void AddNewRequest(Nullable<System.DateTime> dateTime, string serverIPAddress, string clientIPAddress,
                                  Nullable<int> requestLength, string originatingLogFile, string originatingLogFilePath, int FileLineNumber)
        {
            using (var context = new DroneDbEntities())
            {
                context.AddNewRequest(dateTime, serverIPAddress, clientIPAddress, requestLength, originatingLogFile,
                                      originatingLogFilePath, FileLineNumber);
            }
        }
    }

    
}
