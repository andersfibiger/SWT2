using System;
using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ITrackHandler
    {
        void UpdateTracks(object sender, List<TrackData> trackList);
        bool CheckIfWithinBoundary(TrackData Data);
        List<string> GetListOfSeparationEvents();
        event EventHandler<List<TrackData>> TrackHandlerDataHandler;
        void CalculateVelocityAndCompassCourse(List<TrackData> newData);

    }
}