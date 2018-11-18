using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class Logger : ILogger
    {
        //This for the file logging
        // StringBuilder sb = new StringBuilder();
        private List<TrackData> loggingDataForEntered { get; set; }
        private List<TrackData> loggingDataForLeft { get; set; }

        


        public Logger(ITrackHandler trackHandler)
        {
            trackHandler.TrackHandlerDataHandler += LogData;
            trackHandler.SeparationDataHandler += LogSeparation;
            trackHandler.EnteredAirspaceHandler += LogTrackEntered;
            trackHandler.LeftAirspaceHandler += LogTrackLeft;

        }

        private void LogTrackLeft(object sender, List<TrackData> e)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Jeg er kommet ud");
            foreach (var tracklistData in e)
            {
                sb.AppendLine($"At time: {tracklistData.TimeStamp} the following plane: {tracklistData.TagId} left the Airspace");
            }
            File.AppendAllText("logging.txt", sb.ToString());
        }

        private void LogTrackEntered(object sender, List<TrackData> e)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Jeg er kommet ind");
            foreach (var tracklistData in e)
            {
                sb.AppendLine($"At time: {tracklistData.TimeStamp} the following plane: {tracklistData.TagId} entered the Airspace");
            }
            File.AppendAllText("logging.txt", sb.ToString());
        }

        private void LogSeparation(object sender, List<TrackData> e)
        {
            Console.WriteLine("Ups! Der er en for tæt på");
        }

        public void LogData(object sender, List<TrackData> trackList)
        {
            ClearData();

            StringBuilder stringBuilder = new StringBuilder();
        
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

            //stringBuilder.Append($"Track tag: {data}");
            File.AppendAllText("log.txt",stringBuilder.ToString());

        }

        public void ClearData()
        {
            Console.Clear();
        }
    }
}