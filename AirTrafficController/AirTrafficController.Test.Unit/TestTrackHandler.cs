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

        [SetUp]
        public void SetUp()
        {
            //creating the fake seperationHandler
            ISeparationHandler _fakesSeparationHandler = Substitute.For<ISeparationHandler>();
            CalculateCompassCourse _fakeCalculateCompassCourse = Substitute.For<CalculateCompassCourse>();
            CalculateVelocity _fakeCalculateVelocity = Substitute.For<CalculateVelocity>();
            IDecoder _fakeDecoder = Substitute.For<IDecoder>();
            //injecting 
            _uut = new TrackHandler(_fakesSeparationHandler, _fakeCalculateVelocity, _fakeCalculateCompassCourse, _fakeDecoder);
        }


        [Test]

        //Check if _trackList gets updated with new track
        public void UpdateTracks_EmptyBeforeInserting_tracksAreUpdated()
        {
            var info = new TrackData { TagId = "abcd", X = 111 };
            List<TrackData> updatedString = new List<TrackData>();
            updatedString.Add(info);
            _uut.UpdateTracks(null, updatedString);

            Assert.That(updatedString.Contains(info), Is.True);
        }
        
        [Test]
        [Ignore("The method tested only sets data and raises an event with a separation handler. The latter should be tested instead (with a fake)")]
        public void UpdateTracks_OldElementDeleted_TracksAreUpdated()
        {
            //var info1 = new TrackData() {TagId = "abcd", "111", "2019", "12345", "010101" };
            //var info2 = new TrackData() {TagId = "xyz123", "ggg222", "210900110022", "g", "999999" };
            //List<TrackData> updatedString = new List<TrackData>();
            //updatedString.Add(info1);
            //updatedString.Add(info2);
            //updatedString.Remove(info1);
            //_uut.UpdateTracks(null, updatedString);

            //Assert.That(updatedString.Contains(info1), Is.False);
        }

        
        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        public static void WithinBoundary_ValuesInRange_resultIsCorret(int x, int y, int a)
        {
            TrackData trackData = new TrackData() {X = x, Y = y, Altitude = a};
            Assert.That(_uut.CheckIfWithinBoundary(trackData), Is.True);
        }

        //checking for just out of range on the lower and upper boundary for
        // + Boundary value analysis
        [TestCase(9999, 15000, 1000)]
        [TestCase(90001, 15000, 1000)]
        [TestCase(90001, 10000, 500)] //x-postition boundary test
        [TestCase(90000, 9999, 500)] //y-postition boundary test
        [TestCase(90000, 90000, 499)] //y-postition boundary test
        public static void WithInBoundary_XoutOfRange_ResultIsFalse(int x, int y, int a)
        {
            TrackData trackData = new TrackData() { X = x, Y = y, Altitude = a };
            Assert.That(_uut.CheckIfWithinBoundary(trackData), Is.False);
        }


        [Test]
        [Ignore("Needs to be fixed so that it calls a listener method here. Check how the test for TransponderReceiver was made")]
        public void GetSeperationEventList_EmptyTracks_ResultIsNull()
        {
            //_uut.UpdateTracks(null, new List<TrackData>());
            //Assert.That(_uut.GetListOfSeparationEvents(),Is.Null);
        }
        
        
    }
}
