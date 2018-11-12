using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ITrack
    {
        void UpdateTracks(List<TrackData> trackList);
        bool CheckIfWithinBoundary(TrackData Data);
        int GetNavigationalCourse(int posX1, int posY1, int posX2, int posY2);
        List<string> GetSeparationEventsList();
    }
}