/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NUnit.Framework;
using TransponderReceiver;

namespace AirTrafficController.Test.Unit
{
    class TestLogger
    {
        private ILogger _uut;
        

        [SetUp]
        public void Setup()
        {
            _uut = new Logger();
        }

        [Test]
        public void LoggerOutput_TestIfPrintWasSucces()
        {
            TrackData printData = new TrackData() {
                TagId = "ABC123",
                X = 15000,
                Y = 15000,
                Altitude = 10000,
                TimeStamp = DateTime.ParseExact("20151006213456789",
                    "yyyyMMddHHmmssfff",
                    null)
            };

            //test if print (Logger) was succes
            Assert.IsTrue(_uut.LogData(printData));
        }

        [Test]
        public void LoggerOutput_TestIfPrintReturnsFalseWithNoData()
        {
            //test if print (Logger) was false
            Assert.IsFalse(_uut.LogData(null));
            TrackData printData = new TrackData() {TagId = ""};
            Assert.IsFalse(_uut.LogData(printData));
        }
    }
}
*/