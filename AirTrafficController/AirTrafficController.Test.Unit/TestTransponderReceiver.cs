using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;
using NSubstitute;


using NUnit.Framework;
using TransponderReceiver;


namespace AirTrafficController.Test.Unit
{
    public class TestTransponderReceiver
    {
        private List<string> _receivedData;
        //this is the thing we actually want to test
        private ITransponderReceiver _uut;

        [SetUp]
        public void Setup()
        {
            //make the fakes
            _uut = Substitute.For<ITransponderReceiver>();
            _uut.TransponderDataReady += TransponderDataHandler;
        }

       
        [Test]
        public void TransponderDataReady_RaiseEventWithTracks_ShouldReceiveTracks()
        {
            List<String> testData = new List<string>();
            testData.Add("AAA111;630094;83421;12500;20181002204132530");
            testData.Add("BBB111;930094;3421;3000;20181002204132520");
            testData.Add("C1C1C1;9094;43210;500;201810022047872530");

            _uut.TransponderDataReady
                += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));

            Assert.That(_receivedData.Count, Is.EqualTo(3));
        }

        public void TransponderDataHandler(object sender, RawTransponderDataEventArgs data)
        {
            _receivedData = data.TransponderData;
        }
    }
}
