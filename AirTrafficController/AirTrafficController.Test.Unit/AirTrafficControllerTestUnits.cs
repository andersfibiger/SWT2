using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using AirTrafficController.Test.Unit.Stubs;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    public class AirTrafficControllerTestUnits
    {
        private static readonly IDecoder _decoder = new StubDecoder();
        private static readonly ILogger _logger = new StubLogger();
        private static readonly ITrack _track = new StubTrack(new StubSeparationHandler());

        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase(10000,10000,500)]
        [TestCase(90000, 90000, 20000)]
        public static void WithinBoundary_ValuesInRange_resultIsCorret(int x, int y, int a)
        {
            Assert.That(_track.CheckIfWithinBoundary(x,y,a),Is.True);
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
            Assert.That(_track.CheckIfWithinBoundary(x,y,a), Is.False);
        }
    }
}
