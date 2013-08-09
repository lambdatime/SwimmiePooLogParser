using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwimmiePooLogParserDrone.Core.Repositories
{
    public interface IRequestRepository
    {
        void AddNewRequest(Nullable<System.DateTime> dateTime, string serverIPAddress, string clientIPAddress,
                                           Nullable<int> requestLength, string originatingLogFile, string originatingLogFilePath, int FileLineNumber);
    }
}
