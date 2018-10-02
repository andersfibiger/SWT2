using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ITrack
    {
        void UpdateTracks(List<string[]> trackList);
        bool CheckIfWithinBoundary(int positionX, int positionY, int altitude);
        List<string> GetSeparationEventsList();
    }
}