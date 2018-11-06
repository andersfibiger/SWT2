using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ITrack
    {
        void UpdateTracks(List<TrackData> trackList);
        bool CheckIfWithinBoundary(int positionX, int positionY, int altitude);
        double GetHorizontalSpeed(int posX1, int posY1, int posX2, int posY2);
        int GetNavigationalCourse(int posX1, int posY1, int posX2, int posY2);
        List<string> GetSeparationEventsList();
    }
}