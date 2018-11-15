using System;
using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ILogger
    {
        void LogData(object sender, List<TrackData> dataTracks);
        void ClearData();
    }
}