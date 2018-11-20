using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NSubstitute;
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
            ITrackHandler _fakeTrackHandler = Substitute.For<ITrackHandler>();
            _uut = new Logger(_fakeTrackHandler);
        }

        [Test]
        public void LoggerOutput_TestIfPrintWasSucces()
        {
            List<TrackData> printData = new List<TrackData>()
            {
                new TrackData()
                {
                    TagId = "ABC123",
                    X = 15000,
                    Y = 15000,
                    Altitude = 10000,
                    Velocity = 300,
                    CompassCourse = 180,
                    TimeStamp = DateTime.ParseExact("20151006213456789",
                        "yyyyMMddHHmmssfff",
                        null)
                }
            };

            Assert.DoesNotThrow(() => _uut.LogData(null, printData)); //it should not throw an exception because list for logData is not empty
        }

        [Test]
        public void LoggerOutput_TestIfPrintReturnsFalseWithNoData()
        {
            //test if print(Logger) was false
            List<TrackData> emptyData = new List<TrackData>();
            Assert.Throws<ArgumentNullException>(() => _uut.LogData(null, emptyData));
        }
    }
    
}
