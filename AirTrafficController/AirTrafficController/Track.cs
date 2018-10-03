using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class Track : ITrack
    {
        private static readonly int MIN_X = 10000;
        private static readonly int MAX_X = 90000;
        private static readonly int MIN_ALTITUDE = 500;
        private static readonly int MAX_ALTITUDE = 20000;
        private readonly ISeparationHandler _separationHandler;
        private List<string[]> _trackList;
        private List<string> separationEventList;

        public Track(ISeparationHandler separationHandler)
        {
            _separationHandler = separationHandler;
        }

        public void UpdateTracks(List<string[]> trackList)
        {
            _trackList = trackList;
            //separationEventList = _separationHandler.CheckForSeparationEvents(trackList);
        }

        public bool CheckIfWithinBoundary(int positionX, int positionY, int altitude)
        {
            return positionX >= MIN_X && positionX <= MAX_X
                                      && positionY >= MIN_X && positionY <= MAX_X
                                      && altitude >= MIN_ALTITUDE && altitude <= MAX_ALTITUDE;
        }

        public List<string> GetSeparationEventsList()
        {
            throw new System.NotImplementedException();
        }
    }
}