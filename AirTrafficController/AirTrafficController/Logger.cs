using System;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class Logger : ILogger
    {
        public void LogData(string[] dataTracks)
        {
            Console.WriteLine("Track tag: " + dataTracks[0]);
            Console.WriteLine($"(X,Y) position: {dataTracks[1]},{dataTracks[2]}");
            Console.WriteLine("Altitude: " + dataTracks[3]);
            Console.WriteLine("Timestamp: " + dataTracks[4]);
            Console.WriteLine("");
        }
    }
}