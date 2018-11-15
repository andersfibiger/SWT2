using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class Logger : ILogger
    {
        public Logger(ITrackHandler trackHandler)
        {
            trackHandler.TrackHandlerDataHandler += LogData;
        }

        public void LogData(object sender, List<TrackData> trackList)
        {
            ClearData();

            foreach (var dataTracks in trackList)
            {
                if (dataTracks.TagId.Equals(String.Empty))
                {
                    //return false if dataTracks is empty
                    continue;
                }
                Console.WriteLine("Track tag: " + dataTracks.TagId);
                Console.WriteLine($"(X,Y) position: {dataTracks.X},{dataTracks.Y}");
                Console.WriteLine("Altitude: " + dataTracks.Altitude);
                Console.WriteLine("Velocity is: " + dataTracks.Velocity + ": m/s");
                Console.WriteLine("Course is: " + dataTracks.CompassCourse + ": Degrees");
                Console.WriteLine("Timestamp: " + dataTracks.TimeStamp);
                Console.WriteLine("");
            }
        }

        public void ClearData()
        {
            Console.Clear();
        }
    }
}