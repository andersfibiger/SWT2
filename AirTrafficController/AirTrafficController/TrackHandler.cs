using System;
using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class TrackHandler : ITrackHandler
    {
        private static readonly int MIN_X = 10000;
        private static readonly int MAX_X = 90000;
        private static readonly int MIN_ALTITUDE = 500;
        private static readonly int MAX_ALTITUDE = 20000;
        private readonly ISeparationHandler _separationHandler;
        private List<string> _separationEventList;
        private CalculateVelocity _cv;
        private CalculateCompassCourse _cc;

        public event EventHandler<List<TrackData>> TrackHandlerDataHandler;

        private List<TrackData> oldData { get; set; }

        public TrackHandler(ISeparationHandler separationHandler, CalculateVelocity cv, CalculateCompassCourse cc, IDecoder dc)
        {

            _separationHandler = separationHandler;
            _cv = cv;
            _cc = cc;
            dc.DecodedDataHandler += UpdateTracks;
            oldData = new List<TrackData>();
        }

        public void UpdateTracks(object sender, List<TrackData> trackList)
        {
            foreach (var TrackData in trackList)
            {
                if(CheckIfWithinBoundary(TrackData) == true)
                {
                    trackList.Remove(TrackData);
                }
            }
            CalculateVelocityAndCompassCourse(trackList);
            oldData = trackList;
            // TODO raise event for separation handler instead of this
            _separationEventList = _separationHandler.CheckForSeparationEvents(trackList);
            TrackHandlerDataHandler.Invoke(this, trackList);
        }

        public bool CheckIfWithinBoundary(TrackData Data)
        {
            return Data.X >= MIN_X && Data.X <= MAX_X
                                      && Data.Y >= MIN_X && Data.Y <= MAX_X
                                      && Data.Altitude >= MIN_ALTITUDE && Data.Altitude <= MAX_ALTITUDE;
        }

        public void CalculateVelocityAndCompassCourse(List<TrackData> newData)
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

        public List<string> GetListOfSeparationEvents()
        {
            return _separationEventList;
        }
    }
}