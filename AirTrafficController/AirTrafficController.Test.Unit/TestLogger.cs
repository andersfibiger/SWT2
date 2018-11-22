using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly List<TrackData> _tracks = new List<TrackData>(2)
        {
            new TrackData()
            {
                TagId = "ATR423",
                X = 11000,
                Y = 12000,
                Altitude = 1000,
                CompassCourse = 42.39,
                Velocity = 1424.53,
                TimeStamp = DateTime.Now
            },
            new TrackData()
            {
                TagId = "BQA519",
                X = 15000,
                Y = 40000,
                Altitude = 1333,
                CompassCourse = 153.01,
                Velocity = 952.0,
                TimeStamp = DateTime.Now
            }
        };
        private readonly string _pathToLoggingFile = $"logging-{DateTime.Now:yyMMdd-HHmmssfff}.txt";

        [SetUp]
        public void Setup()
        {
            ITrackHandler fakeTrackHandler = Substitute.For<ITrackHandler>();
            _uut = new Logger(fakeTrackHandler, _pathToLoggingFile);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_pathToLoggingFile))
            {
                File.Delete(_pathToLoggingFile);
            }
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
        public void LoggerOutput_TestIfPrintThrowsExceptionWhenDataListIsEmpty()
        {
            //test if print(Logger) was false
            List<TrackData> emptyData = new List<TrackData>();
            Assert.Throws<ArgumentNullException>(() => _uut.LogData(null, emptyData));
        }

        [Test]
        public void LoggerOutput_TrackLeft_IsLoggedToFile()
        {
            TestLoggerOutputToFile(_uut.LogTrackLeftToFile, _tracks, "left the Airspace");
        }

        [Test]
        public void LoggerOutput_TrackEntered_IsLoggedToFile()
        {
            TestLoggerOutputToFile(_uut.LogTrackEnteredToFile, _tracks, "entered the Airspace");
        }

        [Test]
        public void LoggerOutput_TrackSeparation_IsLoggedToFile()
        {
            var separationEvents = new List<string>(2)
            {
                "20151006213456789;ATR423;ABC123",
                "20151006213456152;AQV516;YR5123"
            };
            // Trigger events
            _uut.LogSeparationToFile(null, separationEvents);

            using (var sr = new StreamReader(_pathToLoggingFile))
            {
                string fileContent = sr.ReadToEnd();
                Assert.That(fileContent.Contains("following two planes are too close"), Is.True);
                foreach (var trackData in separationEvents)
                {
                    var separationDataStrings = trackData.Split(';');
                    Assert.That(fileContent.Contains(separationDataStrings[1]), Is.True); // tag id 1
                    Assert.That(fileContent.Contains(separationDataStrings[2]), Is.True); // tag id 2
                    Assert.That(fileContent.Contains(separationDataStrings[0]), Is.True); // time stamp
                }
            }
        }

        private void TestLoggerOutputToFile(Action<object, ICollection<TrackData>> logTrackActionToFile,
            List<TrackData> tracks,
            string contentCompareString)
        {
            logTrackActionToFile(null, tracks);
            using (var sr = new StreamReader(_pathToLoggingFile))
            {
                string fileContent = sr.ReadToEnd();
                Assert.That(fileContent.Contains(contentCompareString), Is.True);
                foreach (var trackData in _tracks)
                {
                    Assert.That(fileContent.Contains(trackData.TagId), Is.True);
                    Assert.That(fileContent.Contains(trackData.TimeStamp.ToString(CultureInfo.CurrentCulture)), Is.True);
                }
            }
        }
    }
    
}
