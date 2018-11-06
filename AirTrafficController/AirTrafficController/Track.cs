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
        private List<TrackData> _trackList;
        private List<string> _separationEventList;

        public Track(ISeparationHandler separationHandler)
        {
            _separationHandler = separationHandler;
        }

        public void UpdateTracks(List<TrackData> trackList)
        {
            _trackList = trackList;
            // TODO raise event for separation handler instead of this
            _separationEventList = _separationHandler.CheckForSeparationEvents(trackList);
        }

        public bool CheckIfWithinBoundary(int positionX, int positionY, int altitude)
        {
            return positionX >= MIN_X && positionX <= MAX_X
                                      && positionY >= MIN_X && positionY <= MAX_X
                                      && altitude >= MIN_ALTITUDE && altitude <= MAX_ALTITUDE;
        }

        public double GetHorizontalSpeed(int posX1, int posY1, int posX2, int posY2)
        {
            throw new System.NotImplementedException();
        }

        public int GetNavigationalCourse(int posX1, int posY1, int posX2, int posY2)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetSeparationEventsList()
        {
            return _separationEventList;
        }
    }
}