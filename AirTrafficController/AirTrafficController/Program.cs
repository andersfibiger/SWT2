using System;
using System.Collections.Generic;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Initiate a virtual airspace with air crafts and return a transponder receiver interface to it.
            var transponderDataReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            var dc = new Decoder(transponderDataReceiver);
            var track = new TrackHandler(new SeparationHandler(), 
                new CalculateVelocity(), new CalculateCompassCourse(), dc);
            var log = new Logger(track);

            while (true)
            {
                System.Threading.Thread.Sleep(1000); // Ugly way of saving some cycles
            }
        }
    }
}
