using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestTrack
    {
        private static Track _uut;
        //Dependencies for Track
        private ISeparationHandler _fakesSeparationHandler;

        [SetUp]
        public void SetUp()
        {
            //creating the fake seperationHandler
            _fakesSeparationHandler = Substitute.For<ISeparationHandler>();
            //injecting 
            _uut = new Track(_fakesSeparationHandler);
        }


        [Test]

        //Check if _trackList gets updated with new track
        public void UpdateTracks_EmptyBeforeInserting_tracksAreUpdated()
        {
            var info = new string[] { "abcd", "111" };
            List<string[]> updatedString = new List<string[]>();
            updatedString.Add(info);
            _uut.UpdateTracks(updatedString);

            Assert.That(updatedString.Contains(info), Is.True);
        }

        [Test]
        public void UpdateTracks_OldElementDeleted_TracksAreUpdated()
        {
            var info1 = new string[] { "abcd", "111","2019","12345","010101" };
            var info2 = new string[] { "xyz123", "ggg222", "210900110022", "g", "999999" };
            List<string[]> updatedString = new List<string[]>();
            updatedString.Add(info1);
            updatedString.Add(info2);
            updatedString.Remove(info1);
            _uut.UpdateTracks(updatedString);
            
            Assert.That(updatedString.Contains(info1), Is.False);
        }


        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        public static void WithinBoundary_ValuesInRange_resultIsCorret(int x, int y, int a)
        {
            Assert.That(_uut.CheckIfWithinBoundary(x, y, a), Is.True);
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
            Assert.That(_uut.CheckIfWithinBoundary(x, y, a), Is.False);
        }


        [Test]

        public void GetSeperationEventList_EmptyTracks_ResultIsNull()
        {
            _uut.UpdateTracks(new List<string[]>());
            Assert.That(_uut.GetSeparationEventsList(),Is.Null);
        }

        
    }
}
