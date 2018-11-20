using System;
using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ITrackHandler
    {
        void UpdateTracks(object sender, List<TrackData> trackList);
        bool CheckIfWithinBoundary(TrackData data);
        //List<string> GetListOfSeparationEvents();
        event EventHandler<ICollection<TrackData>> TrackHandlerDataHandler;
        event EventHandler<ICollection<TrackData>> EnteredAirspaceJustNowHandler;
        event EventHandler<ICollection<TrackData>> EnteredAirspaceWithinTimePeriodHandler;
        event EventHandler<ICollection<TrackData>> LeftAirspaceJustNowHandler;
        event EventHandler<ICollection<TrackData>> LeftAirspaceWithinTimePeriodHandler;
        event EventHandler<ICollection<string>> SeparationJustNowHandler;
        event EventHandler<ICollection<string>> SeparationWithinTimePeriodHandler;
        void CalculateVelocityAndCompassCourse(List<TrackData> newData);

    }
}