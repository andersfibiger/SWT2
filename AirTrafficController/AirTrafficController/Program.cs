using System;
using System.Collections.Generic;
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
            TrafficController tC = new TrafficController(
                new Decoder(),
                new Logger(),
                new Track(new SeparationHandler()),
                transponderDataReceiver);

            while (true)
            {
                System.Threading.Thread.Sleep(1000); // Ugly way of saving some cycles
            }
        }
    }
}
