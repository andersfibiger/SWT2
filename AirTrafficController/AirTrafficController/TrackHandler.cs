using System;
using System.Collections.Generic;
using System.Linq;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class TrackHandler : ITrackHandler
    {
        private const int MIN_X = 10000;
        private const int MIN_Y = 10000;
        private const int MAX_X = 90000;
        private const int MAX_Y = 90000;
        private const int MIN_ALTITUDE = 500;
        private const int MAX_ALTITUDE = 20000;
        private const long TIME_PERIOD_FOR_EVENTS_IN_MS = 5000;
        private readonly ISeparationHandler _separationHandler;
        private readonly CalculateVelocity _cv;
        private readonly CalculateCompassCourse _cc;

        public event EventHandler<ICollection<TrackData>> TrackHandlerDataHandler;
        public event EventHandler<ICollection<TrackData>> EnteredAirspaceJustNowHandler;
        public event EventHandler<ICollection<TrackData>> EnteredAirspaceWithinTimePeriodHandler;
        public event EventHandler<ICollection<TrackData>> LeftAirspaceJustNowHandler;
        public event EventHandler<ICollection<TrackData>> LeftAirspaceWithinTimePeriodHandler;
        public event EventHandler<ICollection<string>> SeparationJustNowHandler;
        public event EventHandler<ICollection<string>> SeparationWithinTimePeriodHandler;

        private List<TrackData> _oldTracksInBoundary = new List<TrackData>();
        private List<string> _oldTracksWithSeparation = new List<string>();
        private IDictionary<TrackData, long> _tracksWhichEnteredAirspaceWithinTimePeriod = new Dictionary<TrackData, long>();
        private IDictionary<TrackData, long> _tracksWhichLeftAirspaceWithinTimePeriod = new Dictionary<TrackData, long>();

        public TrackHandler(ISeparationHandler separationHandler, CalculateVelocity cv, CalculateCompassCourse cc, IDecoder dc)
        {
            _separationHandler = separationHandler;
            _cv = cv;
            _cc = cc;
            dc.DecodedDataHandler += UpdateTracks;
        }

        public void UpdateTracks(object sender, List<TrackData> trackList)
        {
            var currentTimeInMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            CheckIfTracksHaveExpiredEvents(_tracksWhichEnteredAirspaceWithinTimePeriod, currentTimeInMs);
            CheckIfTracksHaveExpiredEvents(_tracksWhichLeftAirspaceWithinTimePeriod, currentTimeInMs);

            var tracksWhichEnteredAirspaceJustNow = new List<TrackData>(trackList.Count);
            var tracksWhichLeftAirspaceJustNow = new List<TrackData>(trackList.Count);

            var tracksInBoundaryList = new List<TrackData>(trackList.Count);
            foreach (var trackData in trackList)
            {
                if (!CheckIfWithinBoundary(trackData)) continue;
                tracksInBoundaryList.Add(trackData);

                if (_oldTracksInBoundary.Any(data => data.TagId.Equals(trackData.TagId))) continue;
                tracksWhichEnteredAirspaceJustNow.Add(trackData);
                _tracksWhichEnteredAirspaceWithinTimePeriod.Add(trackData, currentTimeInMs);
            }

            // If no planes are in boundary, then there's no need to proceed.
            if (tracksInBoundaryList.Count == 0)
            {
                return;
            }

            foreach (var oldTrackData in _oldTracksInBoundary)
            {
                var didTrackLeaveAirspace = true;
                foreach (var newTrackData in tracksInBoundaryList)
                {
                    if (oldTrackData.TagId.Equals(newTrackData.TagId))
                    {
                        didTrackLeaveAirspace = false;
                        break;
                    }
                }

                if (!didTrackLeaveAirspace) continue;
                tracksWhichLeftAirspaceJustNow.Add(oldTrackData);
                _tracksWhichLeftAirspaceWithinTimePeriod.Add(oldTrackData, currentTimeInMs);
            }

            var currentSeparationEventList = _separationHandler.CheckForSeparationEvents(tracksInBoundaryList);
            var newSeparationEventList = new List<string>(currentSeparationEventList.Count);
            foreach (var timeStampAndTagId1AndTagId2 in currentSeparationEventList)
            {
                var newTrackItems = timeStampAndTagId1AndTagId2.Split(';');

                // Check whether we have seen each separation event before
                // by comparing the old tag ids with the new ones.
                if (_oldTracksWithSeparation.Any(oldSeparationEvent =>
                    oldSeparationEvent.Split(';')[1].Equals(newTrackItems[1]) &&
                    oldSeparationEvent.Split(';')[2].Equals(newTrackItems[2]))) continue;
                // If we get here, then it's a new separation event.
                newSeparationEventList.Add(timeStampAndTagId1AndTagId2);
            }

            _oldTracksWithSeparation = currentSeparationEventList;

            EnteredAirspaceJustNowHandler.Invoke(this, tracksWhichEnteredAirspaceJustNow);
            EnteredAirspaceWithinTimePeriodHandler.Invoke(this, _tracksWhichEnteredAirspaceWithinTimePeriod.Keys);
            LeftAirspaceJustNowHandler.Invoke(this, tracksWhichLeftAirspaceJustNow);
            LeftAirspaceWithinTimePeriodHandler.Invoke(this, _tracksWhichLeftAirspaceWithinTimePeriod.Keys);
            SeparationJustNowHandler.Invoke(this, newSeparationEventList);
            SeparationWithinTimePeriodHandler.Invoke(this, currentSeparationEventList);

            CalculateVelocityAndCompassCourse(tracksInBoundaryList);
            _oldTracksInBoundary = tracksInBoundaryList;

            TrackHandlerDataHandler.Invoke(this, tracksInBoundaryList);
        }

        private void CheckIfTracksHaveExpiredEvents(IDictionary<TrackData, long> tracks, long currentTimeInMs)
        {
            var tracksCopy = tracks;
            var tracksWithExpiredEvent = tracks.Where(pair => currentTimeInMs - pair.Value > TIME_PERIOD_FOR_EVENTS_IN_MS).ToArray();
            foreach (var trackData in tracksWithExpiredEvent)
            {
                tracks.Remove(trackData);
            }
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
                foreach (var oldTrack in _oldTracksInBoundary)
                {
                    if (newTrack.TagId == oldTrack.TagId)
                    {
                        _cv.CalcVelocity(oldTrack, newTrack);
                        _cc.CalcCompassCourse(oldTrack, newTrack);
                    }
                }
            }
        }

        //public List<string> GetListOfSeparationEvents()
        //{
        //    return _separationEventList;
        //}
    }
}