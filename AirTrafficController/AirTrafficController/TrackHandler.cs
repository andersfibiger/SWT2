using System;
using System.Collections.Generic;
using System.Linq;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class TrackHandler : ITrackHandler
    {
        private static readonly int MIN_X = 10000;
        private static readonly int MIN_Y = 10000;
        private static readonly int MAX_X = 90000;
        private static readonly int MAX_Y = 90000;
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
            List<TrackData> tracksInBoundaryList = new List<TrackData>(trackList.Count);
            foreach (var trackData in trackList)
            {
                if (CheckIfWithinBoundary(trackData))
                {
                    tracksInBoundaryList.Add(trackData);
                }
            }

            if (tracksInBoundaryList.Count == 0)
            {
                return;
            }
            CalculateVelocityAndCompassCourse(tracksInBoundaryList);
            oldData = tracksInBoundaryList;
            // TODO raise event for separation handler instead of this
            _separationEventList = _separationHandler.CheckForSeparationEvents(tracksInBoundaryList);
            TrackHandlerDataHandler.Invoke(this, tracksInBoundaryList);
        }

        public bool CheckIfWithinBoundary(TrackData data)
        {
            return data.X >= MIN_X && data.X <= MAX_X
                                      && data.Y >= MIN_Y && data.Y <= MAX_Y
                                      && data.Altitude >= MIN_ALTITUDE && data.Altitude <= MAX_ALTITUDE;
        }

        public void CalculateVelocityAndCompassCourse(List<TrackData> newData)
        {
            foreach (var newTrack in newData)
            {
                foreach (var oldTrack in oldData)
                {
                    if (newTrack.TagId == oldTrack.TagId)
                    {
                        _cv.CalcVelocity(oldTrack, newTrack);
                        _cc.CalcCompassCourse(oldTrack, newTrack);
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