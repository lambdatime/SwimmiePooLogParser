using System.Collections.Generic;
using System.Linq;

namespace SwimmiePooLogParserDispatcher.Core.Repositories
{
    public interface IDroneRepository
    {
        IEnumerable<Drone> GetAll();

        Drone Get(int id);

        int AddDrone(Drone drone);

        void DeleteDrone(Drone drone);

        void UpdateDrone(Drone drone);
    }

    public interface ICurrentParseLinesRepository
    {
        bool FileCompletelyParsed();

        IEnumerable<CurrentParseLine> GetWorkLoadByDroneId(int droneId);

        IEnumerable<CurrentParseLine> GetTopParseLinesToAssign(int topCount);

        void RemoveAll(string file, string path);

        void AddRange(IEnumerable<CurrentParseLine> currentParseLines);

        void Update(CurrentParseLine currentParseLine);
    }
}