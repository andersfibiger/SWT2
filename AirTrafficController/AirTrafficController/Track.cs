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
        private CalculateVelocity _cv;
        private CalculateCompassCourse _cc;
        public List<TrackData> oldData { get; set; }

        public Track(ISeparationHandler separationHandler, CalculateVelocity cv, CalculateCompassCourse cc)
        {
            _separationHandler = separationHandler;
            _cv = cv;
            _cc = cc;
            oldData = new List<TrackData>();
        }

        public void UpdateTracks(List<TrackData> trackList)
        {
            foreach (var TrackData in trackList)
            {
                if(CheckIfWithinBoundary(TrackData) == true)
                {
                    trackList.Remove(TrackData);
                }
            }
            GetVelocityAndCompassCourse(trackList);
            oldData = trackList;
            _trackList = trackList;
            // TODO raise event for separation handler instead of this
            _separationEventList = _separationHandler.CheckForSeparationEvents(trackList);
            
        }

        public bool CheckIfWithinBoundary(TrackData Data)
        {
            return Data.X >= MIN_X && Data.X <= MAX_X
                                      && Data.Y >= MIN_X && Data.Y <= MAX_X
                                      && Data.Altitude >= MIN_ALTITUDE && Data.Altitude <= MAX_ALTITUDE;
        }

        public void GetVelocityAndCompassCourse(List<TrackData> newData)
        {
            for(int i = 0; i<newData.Count; i++)
            {
                for (int j = 0; j  < oldData.Count; j++)
                {
                    if (newData[i].TagId == oldData[j].TagId)
                    {
                        _cv.CalcVelocity(oldData[i], newData[j]);
                        _cc.CalcCompassCourse(oldData[i], newData[j]);
                    }
                }
            }
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