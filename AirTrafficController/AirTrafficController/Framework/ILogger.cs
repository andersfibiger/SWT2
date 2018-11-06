using System;

namespace AirTrafficController.Framework
{
    public interface ILogger
    {
        bool LogData(TrackData dataTracks);
        void ClearData();
    }
}