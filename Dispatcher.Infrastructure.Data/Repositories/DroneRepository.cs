using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SwimmiePooLogParserDispatcher.Core;
using SwimmiePooLogParserDispatcher.Core.Repositories;
using SwimmiePooLogParserDispatcher.Infrastructure.Data;

namespace Dispatcher.Infrastructure.Data
{
    public class CurrentParseLinesRepository : ICurrentParseLinesRepository
    {
        public bool FileCompletelyParsed()
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                var unassignedCount = context.CurrentParseLines.Where(x => x.DroneId == null).Count();
                var totalCount = context.CurrentParseLines.Count();
                return (totalCount > 0 && unassignedCount == 0);
            }
        }

        public IEnumerable<CurrentParseLine> GetTopParseLinesToAssign(int topCount)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                return context.CurrentParseLines.Where(x => x.DroneId == null).OrderBy(x => x.LineNumber).Take(topCount).ToList();
            }
        }

        public IEnumerable<CurrentParseLine> GetWorkLoadByDroneId(int droneId)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                return context.CurrentParseLines.Where(x => x.DroneId == droneId).ToList();
            }
        }

        public void RemoveAll(string file, string path)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                context.ClearTheParsed(file, path);
                //foreach (var currentParseLine in context.CurrentParseLines.ToList())
                //{
                //    context.CurrentParseLines.Remove(currentParseLine);
                //}
                //context.SaveChanges();
            }  
        }

        public void AddRange(IEnumerable<CurrentParseLine> currentParseLines)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                foreach (var currentParseLine in currentParseLines)
                {
                    context.AddParseLine(currentParseLine.FileName, currentParseLine.Path, currentParseLine.LineNumber, currentParseLine.Line);
                    //context.CurrentParseLines.Add(currentParseLine);
                    //context.SaveChanges();
                }
                
                
            }
        }

        public void Update(CurrentParseLine currentParseLine)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                var queriedParseLine = (from cpl in context.CurrentParseLines
                                    where cpl.Id == currentParseLine.Id
                                    select cpl).FirstOrDefault();
                if (queriedParseLine != null)
                {
                    queriedParseLine.DroneId = currentParseLine.DroneId;
                    context.SaveChanges();
                }
            }
        }
    }
  
   

    public class DroneRepository : IDroneRepository
    {
        public IEnumerable<Drone> GetAll()
        {
            using(var context = new SwimmiePooLogParserDispatcherEntities())
            {
                return context.Drones.ToList();
            }
        }

        public Drone Get(int id)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                return context.Drones.FirstOrDefault(d => d.DroneId == id);
            }
        }

        public int AddDrone(Drone drone)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                context.Drones.Add(drone);
                context.SaveChanges();
            }
            return drone.DroneId;
        }

        public void DeleteDrone(Drone drone)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                var contextDrone = context.Drones.FirstOrDefault(x => x.DroneId == drone.DroneId);
                if (contextDrone != null)
                {
                    context.Drones.Remove(contextDrone);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateDrone(Drone drone)
        {
            using (var context = new SwimmiePooLogParserDispatcherEntities())
            {
                var queriedDrone = (from d in context.Drones
                                   where d.DroneId == drone.DroneId
                                   select d).FirstOrDefault();
                if (queriedDrone != null)
                {
                    queriedDrone.Title = drone.Title;
                    queriedDrone.BaseUrl = drone.BaseUrl;
                    queriedDrone.SortOrder = drone.SortOrder;
                    context.SaveChanges();
                }
            }
        }
    }
}
