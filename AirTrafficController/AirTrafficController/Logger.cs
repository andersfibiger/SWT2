using System;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class Logger : ILogger
    {
        public bool LogData(TrackData dataTracks)
        {
            if (dataTracks == null || dataTracks.TagId.Equals(String.Empty))
            {
                //return false if dataTracks is empty
                return false;
            }
            
            Console.WriteLine("Track tag: " + dataTracks.TagId);
            Console.WriteLine($"(X,Y) position: {dataTracks.X},{dataTracks.Y}");
            Console.WriteLine("Altitude: " + dataTracks.Altitude);
            Console.WriteLine("Velocity is: " + dataTracks.Velocity + ": m/s");
            Console.WriteLine("Course is: " + dataTracks.CompassCourse + ": Degrees");
            Console.WriteLine("Timestamp: " + dataTracks.TimeStamp);
            Console.WriteLine(""); 

            //return true if print succes
            return true;
        }

        public void ClearData()
        {
            Console.Clear();
        }
    }
}