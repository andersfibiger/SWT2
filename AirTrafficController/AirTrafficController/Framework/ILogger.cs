using System;
using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ILogger
    {
        void LogTrackLeftToFile(object sender, ICollection<TrackData> dataTracks);
        void LogTrackLeftToConsole(object sender, ICollection<TrackData> dataTracks);
        void LogTrackEnteredToFile(object sender, ICollection<TrackData> dataTracks);
        void LogTrackEnteredToConsole(object sender, ICollection<TrackData> dataTracks);
        void LogSeparationToFile(object sender, ICollection<string> dataTracks);
        void LogSeparationToConsole(object sender, ICollection<string> dataTracks);
        void LogData(object sender, ICollection<TrackData> dataTracks);
        void ClearData();
    }
}