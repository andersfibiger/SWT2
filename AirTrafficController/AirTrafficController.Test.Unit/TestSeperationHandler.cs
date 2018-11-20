using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestSeperationHandler
    {
        private static ISeparationHandler _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new SeparationHandler(); 
        }

        private List<TrackData> createAndReturnTrackList(int trackX1, int trackY1, int trackZ1, int trackX2, int trackY2,
            int trackZ2)
        {
            return new List<TrackData>()
            {
                new TrackData() {TagId = "TESTAAA", X = trackX1, Y= trackY1, Altitude = trackZ1},
                new TrackData() {TagId = "TESTBBB", X = trackX2, Y= trackY2, Altitude = trackZ2}
            };
        }

        [TestCase(0, 0, 100, 1256, 62124, 600)]
        [TestCase(0, 0, 100, 4000, 4000, 200)]
        public void SeparationEvents_TracksNotCloseEnough_ResultIsNoSeparation(
            int trackX1, int trackY1, int trackZ1, 
            int trackX2, int trackY2, int trackZ2)
        {
            var tracksNoSeparationList = createAndReturnTrackList(trackX1, trackY1, trackZ1, trackX2, trackY2, trackZ2);
            Assert.That(_uut.GetListOfSeparationEvents(tracksNoSeparationList), Is.Empty);
        }
        
        [TestCase(0, 0, 100, 5000, 0, 100)] // test boundary value for x
        [TestCase(0, 0, 100, 6000, 0, 400)] // test boundary value for z
        [TestCase(0, 0, 100, 4999, 100, 100)] // test boundary value for x and y
        [TestCase(0, 0, 100, 5000, 0, 400)] // test boundary value for x and z
        [TestCase(0, 0, 100, 0, 5000, 400)] // test boundary value for y and z
        [TestCase(0, 0, 100, 4999, 100, 400)] // test boundary value for x, y and z
        [TestCase(0, 0, 100, -5000, 0, 100)] // test boundary value for negative x
        [TestCase(0, 0, 100, 0, -5000, 100)] // test boundary for negative y
        public void SeparationEvents_TracksNotCloseEnoughBoundary_ResultIsNoSeparation(
            int trackX1, int trackY1, int trackZ1,
            int trackX2, int trackY2, int trackZ2)
        {
            var tracksNoSeparationList = createAndReturnTrackList(trackX1, trackY1, trackZ1, trackX2, trackY2, trackZ2);
            Assert.That(_uut.GetListOfSeparationEvents(tracksNoSeparationList), Is.Empty);
        }

        [TestCase(0, 0, 100, 10, 10, 99)]
        [TestCase(0, 0, 100, 4999, 0, 99)] // boundary value for x
        [TestCase(0, 0, 100, 0, 4999, 99)] // boundary value for y
        [TestCase(0, 0, 100, 0, 40, 399)] // boundary value for z
        [TestCase(0, 0, 100, -4999, 0, 99)] // boundary value for negative x
        public void SeparationEvents_TracksCloseEnough_ResultIsSeparation(
            int trackX1, int trackY1, int trackZ1,
            int trackX2, int trackY2, int trackZ2)
        {
            var tracksWithSeparationList = createAndReturnTrackList(trackX1, trackY1, trackZ1, trackX2, trackY2, trackZ2);
            Assert.That(_uut.GetListOfSeparationEvents(tracksWithSeparationList), Has.Count.EqualTo(1));
        }
    }
}
