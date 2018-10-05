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
    public class TransponderReceiverTest
    {
        private TrafficController _uut;

        //this is the thing we actually want to test
        private ITransponderReceiver _fakeTransponderReceiver;
        
        //these are for integrationtest, but will not be tested for now
        private IDecoder Decoder;
        private ILogger _fakeLogger;
        private ITrack _fakeTrack;


        [SetUp]
        public void Setup()
        {
            //make the fakes
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            Decoder = new Decoder();
            _fakeLogger = Substitute.For<ILogger>();
            _fakeTrack = Substitute.For<ITrack>();

            //inject the fakes in uut
            _uut = new TrafficController(Decoder, _fakeLogger, _fakeTrack, _fakeTransponderReceiver);
        }

       
        [Test]
        public void _fakeTransponderReceiver_3Tracks()
        {
            List<String> testData = new List<string>();
            testData.Add("AAA111;630094;83421;12500;20181002204132530");
            testData.Add("BBB111;930094;3421;3000;20181002204132520");
            testData.Add("C1C1C1;9094;43210;500;201810022047872530");

            _fakeTransponderReceiver.TransponderDataReady
                += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));
            // TODO Why doesnt it work...
            Assert.That(_uut.numberOfTracks,Is.EqualTo(3));
        }
        
    }
}
