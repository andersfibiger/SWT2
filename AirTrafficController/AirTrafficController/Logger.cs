using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const string LoggingFileName = "..\\..\\logging.txt";

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
            foreach (var trackData in e)
            {
                sb.AppendLine($"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} left the Airspace");
            }
            File.AppendAllText(LoggingFileName, sb.ToString());
        }

        private void LogTrackEntered(object sender, List<TrackData> e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var trackData in e)
            {
                sb.AppendLine($"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} entered the Airspace");
            }
            File.AppendAllText(LoggingFileName, sb.ToString());
        }

        private void LogSeparation(object sender, List<TrackData> e)
        {
            Console.WriteLine("Ups! Der er en for tæt på");
        }

        public void LogData(object sender, List<TrackData> trackList)
        {
            ClearData();

            StringBuilder stringBuilder = new StringBuilder();

            if (!trackList.Any())
            {
                throw new ArgumentNullException("Logger.LogData: The given list is empty");
            }

            foreach (var trackData in trackList)
            {             
                Console.WriteLine("Track tag: " + trackData.TagId);
                Console.WriteLine($"(X,Y) position: {trackData.X},{trackData.Y}");
                Console.WriteLine("Altitude: " + trackData.Altitude);
                Console.WriteLine("Velocity is: " + trackData.Velocity + ": m/s");
                Console.WriteLine("Course is: " + trackData.CompassCourse + ": Degrees");
                Console.WriteLine("Timestamp: " + trackData.TimeStamp);
                Console.WriteLine("");
            }
            File.AppendAllText("log.txt",stringBuilder.ToString());

        }

        public void ClearData()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException e)
            {
                //Do nothing. This should only occur when unit testing. Maybe because of NSubstitute.
            }
            }
    }
}