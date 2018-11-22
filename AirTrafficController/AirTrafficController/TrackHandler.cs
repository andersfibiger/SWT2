using System;
using System.Collections.Generic;
using System.Linq;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class TrackHandler : ITrackHandler
    {
        private const int MinX = 10000;
        private const int MinY = MinX;
        private const int MaxX = 90000;
        private const int MaxY = MaxX;
        private const int MinAltitude = 500;
        private const int MaxAltitude = 20000;
        private const long TimePeriodForEventsInMs = 5000;
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

        public List<TrackData> _oldTracksInBoundary = new List<TrackData>();
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
            RemoveTracksWithExpiredEvents(_tracksWhichEnteredAirspaceWithinTimePeriod, currentTimeInMs);
            RemoveTracksWithExpiredEvents(_tracksWhichLeftAirspaceWithinTimePeriod, currentTimeInMs);

            var tracksWhichEnteredAirspaceJustNow = new List<TrackData>(trackList.Count);
            var tracksWhichLeftAirspaceJustNow = new List<TrackData>(trackList.Count);

            var tracksInBoundaryList = new List<TrackData>(trackList.Count);
            
            foreach (var trackData in trackList)
            {
                if (!IsTrackWithinBoundary(trackData)) continue;
                tracksInBoundaryList.Add(trackData);

                if (IsTrackOld(_oldTracksInBoundary, trackData.TagId)) continue;
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

            var currentSeparationEventList = _separationHandler.GetListOfSeparationEvents(tracksInBoundaryList);
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
            CalculateVelocityAndCompassCourse(_oldTracksInBoundary, tracksInBoundaryList);
            _oldTracksInBoundary = tracksInBoundaryList;

            // Invoke events on all event handlers, if any.
            EnteredAirspaceJustNowHandler?.Invoke(this, tracksWhichEnteredAirspaceJustNow);
            EnteredAirspaceWithinTimePeriodHandler?.Invoke(this, _tracksWhichEnteredAirspaceWithinTimePeriod.Keys);
            LeftAirspaceJustNowHandler?.Invoke(this, tracksWhichLeftAirspaceJustNow);
            LeftAirspaceWithinTimePeriodHandler?.Invoke(this, _tracksWhichLeftAirspaceWithinTimePeriod.Keys);
            SeparationJustNowHandler?.Invoke(this, newSeparationEventList);
            SeparationWithinTimePeriodHandler?.Invoke(this, currentSeparationEventList);
            TrackHandlerDataHandler?.Invoke(this, tracksInBoundaryList);
        }

        public bool IsTrackOld(List<TrackData> oldTracksInBoundary, string trackDataTagId)
        {
            return _oldTracksInBoundary.Any(data => data.TagId.Equals(trackDataTagId));
        }

        public void RemoveTracksWithExpiredEvents(IDictionary<TrackData, long> tracks, long currentTimeInMs)
        {
            var tracksWithExpiredEvent = tracks
                .Where(pair => currentTimeInMs - pair.Value > TimePeriodForEventsInMs)
                .ToArray();

            foreach (var trackData in tracksWithExpiredEvent)
            {
                tracks.Remove(trackData);
            }
        }

        public bool IsTrackWithinBoundary(TrackData data)
        {
            return data.X >= MinX && data.X <= MaxX
                                      && data.Y >= MinY && data.Y <= MaxY
                                      && data.Altitude >= MinAltitude && data.Altitude <= MaxAltitude;
        }

        public void CalculateVelocityAndCompassCourse(List<TrackData> oldData, List<TrackData> newData)
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
    }
}