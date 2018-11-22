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
        private readonly string _pathToLoggingFile;
        private string _tracksLeftLogString = "";
        private string _tracksEnteredLogString = "";
        private string _tracksSeparationLogString = "";

        public Logger(ITrackHandler trackHandler, string pathToLoggingFile)
        {
            trackHandler.TrackHandlerDataHandler += LogData;
            trackHandler.LeftAirspaceJustNowHandler += LogTrackLeftToFile;
            trackHandler.LeftAirspaceWithinTimePeriodHandler += LogTrackLeftToConsole;
            trackHandler.EnteredAirspaceJustNowHandler += LogTrackEnteredToFile;
            trackHandler.EnteredAirspaceWithinTimePeriodHandler += LogTrackEnteredToConsole;
            trackHandler.SeparationJustNowHandler += LogSeparationToFile;
            trackHandler.SeparationWithinTimePeriodHandler += LogSeparationToConsole;
            _pathToLoggingFile = pathToLoggingFile;
        }

        public void LogTrackLeftToFile(object sender, ICollection<TrackData> e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var trackData in e)
            {
                sb.AppendLine($"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} left the Airspace.");
            }
            File.AppendAllText(_pathToLoggingFile, sb.ToString());
        }

        public void LogTrackLeftToConsole(object sender, ICollection<TrackData> dataTracks)
        {
            foreach (var trackData in dataTracks)
            {
                _tracksLeftLogString += $"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} left the Airspace.\n";
            }
        }

        public void LogTrackEnteredToFile(object sender, ICollection<TrackData> e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var trackData in e)
            {
                sb.AppendLine($"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} entered the Airspace.");
            }
            File.AppendAllText(_pathToLoggingFile, sb.ToString());
        }

        public void LogTrackEnteredToConsole(object sender, ICollection<TrackData> dataTracks)
        {
            foreach (var trackData in dataTracks)
            {
                _tracksEnteredLogString += $"At time: {trackData.TimeStamp} the following plane: {trackData.TagId} entered the Airspace.\n";
            }
        }

        public void LogSeparationToFile(object sender, ICollection<string> e)
        {
            var sb = new StringBuilder();
            foreach (var timeStampAndTagId1AndTagId2 in e)
            {
                var trackItems = timeStampAndTagId1AndTagId2.Split(';');
                sb.AppendLine($"At time: {trackItems[0]} the following two planes are too close to each other: " +
                              $"{trackItems[1]} and {trackItems[2]}.");
            }
            File.AppendAllText(_pathToLoggingFile, sb.ToString());
        }

        public void LogSeparationToConsole(object sender, ICollection<string> dataTracks)
        {
            foreach (var timeStampAndTagId1AndTagId2 in dataTracks)
            {
                var trackItems = timeStampAndTagId1AndTagId2.Split(';');
                _tracksSeparationLogString += $"At time: {trackItems[0]} the following two planes are too close to each other: " +
                              $"{trackItems[1]} and {trackItems[2]}.\n";
            }
        }

        public void LogData(object sender, ICollection<TrackData> trackList)
        {
            ClearData();

            // Print events first and track data later.
            if (!_tracksEnteredLogString.Equals(string.Empty))
            {
                Console.WriteLine(_tracksEnteredLogString);
                Console.WriteLine();
            }
            if (!_tracksLeftLogString.Equals(string.Empty))
            {
                Console.WriteLine(_tracksLeftLogString);
                Console.WriteLine();
            }
            if (!_tracksSeparationLogString.Equals(string.Empty))
            {
                Console.WriteLine(_tracksSeparationLogString);
                Console.WriteLine();
            }

            var stringBuilder = new StringBuilder();

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

            _tracksEnteredLogString = "";
            _tracksLeftLogString = "";
            _tracksSeparationLogString = "";
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