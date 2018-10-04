using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NSubstitute;


using NUnit.Framework;
using TransponderReceiver;


namespace AirTrafficController.Test.Unit
{
    public class AirTrafficControllerTestUnits
    {
        // TODO TrafficController is for integration testing
        //private static TrafficController tC;
        private static readonly IDecoder _decoder = new Decoder();
        private static readonly ILogger _logger = new Logger();
        private static readonly ISeparationHandler _separationHandler = new SeparationHandler();
        private static readonly ITrack _track = new Track(_separationHandler);
        private ITransponderReceiver _fakeTransponderReceiver;

        

        [SetUp]
        public void Setup()
        {
            // TODO When using integration test, uncomment the following line
            //tC = new TrafficController(_decoder, _logger, _track, _transponderReceiver);

            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
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

        public void TestReception()
        {
            List<String> testData = new List<string>();
            testData.Add("AAA111;630094;83421;12500;20181002204132530");
            testData.Add("BBB111;930094;3421;3000;20181002204132520");
            testData.Add("C1C1C1;9094;43210;500;201810022047872530");

            _fakeTransponderReceiver.TransponderDataReady
                += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));

            _decoder.DecodeData(testData[0]);


        }
    }
}
