using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace AirTrafficController
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Initiate a virtual airspace with air crafts and return a transponder receiver interface to it.
            var transReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            // Subscribe to transponder events with a custom callback method.
            transReceiver.TransponderDataReady += TransReceiverOnTransponderDataReady;
            

            while (true)
            {
                System.Threading.Thread.Sleep(1000); // Ugly way of saving some cycles
            }

            Console.WriteLine("Press key");
            Console.ReadKey();
        }

        private static void TransReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e)
        {
            Console.Clear();
            /* TODO remember to check whether the (new) list contains data which has already been printed before.
             Clearing the list does NOT work as it auto-generates everything within the current airspace.
             */

            // We iterate over every aircraft.
            foreach (string airCraftsData in e.TransponderData)
            {

                // For each aircraft, split the string into separate data items. 
                string[] arrayOfAircraftDataItems = airCraftsData.Split(';');
                if (checkIfWithinBoundary(Int32.Parse(arrayOfAircraftDataItems[1]), 
                    Int32.Parse(arrayOfAircraftDataItems[2]), 
                    Int32.Parse(arrayOfAircraftDataItems[3])))
                {
                    Console.WriteLine("Track tag: " + arrayOfAircraftDataItems[0]);
                    Console.WriteLine($"(X,Y) position: {arrayOfAircraftDataItems[1]},{arrayOfAircraftDataItems[2]})");
                    Console.WriteLine("Altitude: " + arrayOfAircraftDataItems[3]);
                    Console.WriteLine("Timestamp: " + arrayOfAircraftDataItems[4]);
                    Console.WriteLine("");
                }
            }
        }

        public static bool checkIfWithinBoundary(int positionX, int positionY, int altitude)
        {
            return positionX >= 10000 && positionX <= 90000
                                      && positionY >= 10000 && positionY <= 90000 
                                      && altitude >=500 && altitude <=20000;
        }
    }
}
    