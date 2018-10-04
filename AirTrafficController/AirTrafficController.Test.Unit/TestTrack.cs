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


        //[Test]
        //public void


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
    }
}
