using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestTrack
    {
        private static TrackHandler _uut;
        private readonly List<TrackData> _tracks = new List<TrackData>()
        {
            new TrackData() {TagId = "ABC123"},
            new TrackData() {TagId = "DEF456"},
            new TrackData() {TagId = "GHI789"}
        };

        [SetUp]
        public void SetUp()
        {
            //creating the fake seperationHandler
            ISeparationHandler _fakesSeparationHandler = Substitute.For<ISeparationHandler>();
            _fakesSeparationHandler.GetListOfSeparationEvents(null).ReturnsForAnyArgs(info => new List<string>());

            CalculateCompassCourse _fakeCalculateCompassCourse = Substitute.For<CalculateCompassCourse>();
            CalculateVelocity _fakeCalculateVelocity = Substitute.For<CalculateVelocity>();
            // TODO method is called before tests for some reason, so args are null.
            //_fakeCalculateCompassCourse.When(course => course.CalcCompassCourse(Arg.Any<TrackData>(),
            //        Arg.Any<TrackData>()))
            //    .Do(info => info[0] = new TrackData()
            //    {
            //        CompassCourse = 123.45
            //    });

            IDecoder _fakeDecoder = Substitute.For<IDecoder>();
            //injecting 
            _uut = new TrackHandler(_fakesSeparationHandler, _fakeCalculateVelocity, _fakeCalculateCompassCourse, _fakeDecoder);
        }

        [Test]
        public void CalculateVelocityAndCompassCourse_SetsTheCorrectVelocityAndCourse()
        {
            var oldTrackDatas = new List<TrackData>() { new TrackData() { TagId = "ABC123" }};
            var newTrackDatas = new List<TrackData>() { new TrackData() { TagId = "ABC123" } };
            _uut.CalculateVelocityAndCompassCourse(oldTrackDatas, newTrackDatas);
            Assert.That(oldTrackDatas[0].Velocity, Is.EqualTo(0.0));
            Assert.That(oldTrackDatas[0].CompassCourse, Is.EqualTo(0.0));
        }

        [Test]
        //Check if _trackList gets updated with new track
        public void UpdateTracks_EmptyBeforeInserting_TracksAreUpdated()
        {
            var info = new TrackData { TagId = "abcd", X = 11000, Y = 11000, Altitude = 1000};
            List<TrackData> updatedString = new List<TrackData>() {info};
            ICollection<TrackData> newTracksEventList = new List<TrackData>(updatedString.Count);
            
            // Add listener to the event handler which notifies us for new tracks entering the airspace.
            _uut.EnteredAirspaceJustNowHandler += (sender, datas) => newTracksEventList = datas;

            _uut.UpdateTracks(null, updatedString);
            Assert.That(newTracksEventList.Contains(info), Is.True);
        }
        
        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        public static void WithinBoundary_ValuesInRange_ResultIsTrue(int x, int y, int a)
        {
            TrackData trackData = new TrackData() {X = x, Y = y, Altitude = a};
            Assert.That(_uut.IsTrackWithinBoundary(trackData), Is.True);
        }

        //checking for just out of range on the lower and upper boundary for
        // + Boundary value analysis
        [TestCase(9999, 15000, 1000)]
        [TestCase(90001, 15000, 1000)]
        [TestCase(90001, 10000, 500)] //x-postition boundary test
        [TestCase(90000, 9999, 500)] //y-postition boundary test
        [TestCase(90000, 90000, 499)] //y-postition boundary test
        public static void WithinBoundary_XoutOfRange_ResultIsFalse(int x, int y, int a)
        {
            TrackData trackData = new TrackData() { X = x, Y = y, Altitude = a };
            Assert.That(_uut.IsTrackWithinBoundary(trackData), Is.False);
        }

        [Test]
        public void GetSeperationEventList_EmptyTracks_ResultIsNull()
        {
            ICollection<string> separationEvents = new List<string>();
            _uut.SeparationJustNowHandler += (sender, collection) => separationEvents = collection;
            _uut.UpdateTracks(null, new List<TrackData>());
            Assert.That(separationEvents,Is.Empty);
        }

        [Test]
        public void RemoveTracksWithExpiredEvents_EmptyTracks_DoesNothing()
        {
            var emptyTrackEventList = new Dictionary<TrackData, long>();
            _uut.RemoveTracksWithExpiredEvents(emptyTrackEventList, 0);
            Assert.That(emptyTrackEventList, Is.Empty);
        }

        [TestCase(0, 0, 1)]
        [TestCase(20, 100, 1)]
        [TestCase(0, 5000, 1)]
        [TestCase(0, 5001, 0)]
        public void RemoveTracksWithExpiredEvents_TracksWithExpiredEvents_RemovesOnlyExpiredEvents(
            long oldTimeInMs, long currentTimeInMs, int expectedNoTracksLeft)
        {
            var trackEventList = new Dictionary<TrackData, long>()
            {
                {new TrackData(), oldTimeInMs}
            };
            _uut.RemoveTracksWithExpiredEvents(trackEventList, currentTimeInMs);
            Assert.That(trackEventList.Count, Is.EqualTo(expectedNoTracksLeft));
        }

        [TestCase("ABC123", true)]
        [TestCase("DEF456", true)]
        [TestCase("GHI789", true)]
        [TestCase("HSW415", false)]
        [TestCase("ABC12", false)]
        public void TrackExistsInBoundary_OldAndNewTracks_ReturnsCorrectly(string tagId, bool expectedValue)
        {
            Assert.That(_uut.DoesTrackAlreadyExistInAirspace(_tracks, tagId), Is.EqualTo(expectedValue));
        }
    }
}
