using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using AirTrafficController.Framework;
using AirTrafficController.Calculating;

namespace AirTrafficController.Test.integration
{
    public class IntegrationTest1
    {
        //
        private ISeparationHandler _separationHandler;
        private CalculateCompassCourse _calculateCompassCource;
        private IDecoder _fakeDecoder;
        private CalculateVelocity _calcVelocity;
        private TrackHandler _sut;

        private ICollection<TrackData> Tracksaver;
        //Data to use in test
        private TrackData _track1;
        private TrackData _track2;
        private TrackData _track3;
        private TrackData _CourseTrack;
        private TrackData _conflictingTrack;
        private TrackData _conflictingTrack2;
        private List<TrackData> _tracklist1;
        private List<TrackData> _tracklist2;
        private List<TrackData> _tracklist3;



        [SetUp]
        public void Setup()
        {
            _separationHandler = new SeparationHandler();
            _calculateCompassCource = new CalculateCompassCourse();
            _calcVelocity = new CalculateVelocity();
            _fakeDecoder = Substitute.For<IDecoder>();

            _sut = new TrackHandler(_separationHandler,_calcVelocity,_calculateCompassCource,_fakeDecoder);
            _tracklist1 = new List<TrackData>();
            _tracklist2 = new List<TrackData>();
            _tracklist3 = new List<TrackData>();


            _track1 = new TrackData
            {
                TagId = "ABCDE",
                X = 90000,
                Y = 90000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 18)
            };

            _track2 = new TrackData
            {
                TagId = "ABCDE",
                X = 90000,
                Y = 80000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 20)
            };
            _track3 = new TrackData
            {
                TagId = "ABCDE",
                X = 90000,
                Y = 60000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 22)
            };
            _CourseTrack = new TrackData
            {
                TagId = "ABCDE",
                X = 80000,
                Y = 60000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 22)
            };

            _conflictingTrack = new TrackData
            {
                TagId = "AAAAA",
                X = 9000,
                Y = 9000,
                Altitude = 100,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 18)
            };
            _conflictingTrack = new TrackData
            {
                TagId = "AAAAB",
                X = 8900,
                Y = 8900,
                Altitude = 99,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 19)
            };
        }


        [Test]
        public void TestCalculateVelocity_VelocityIs5000()
        {
            _tracklist1.Add(_track1);
            _tracklist2.Add(_track2);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist2);

            // _sut.CalculateVelocityAndCompassCourse(_tracklist1, _tracklist2);
            Assert.That(Math.Round(Tracksaver.ElementAt(0).Velocity), Is.EqualTo(5000));
        }
        [Test]
        public void TestCalculateVelocity_VelocityIs10000()
        {
            _tracklist1.Add(_track1);
            _tracklist2.Add(_track2);
            _tracklist3.Add(_track3);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist2);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist3);

            Assert.That(Math.Round(Tracksaver.ElementAt(0).Velocity), Is.EqualTo(10000));
        }
        [Test]
        public void TestCalculateVelocity_VelocityIs0()
        {
            _tracklist1.Add(_track1);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);

            // _sut.CalculateVelocityAndCompassCourse(_tracklist1, _tracklist2);
            Assert.That(Math.Round(Tracksaver.ElementAt(0).Velocity), Is.EqualTo(0));
        }
        [Test]
        public void TestCalculateCompassCourse_compassIs90()
        {
            _tracklist1.Add(_track1);
            _tracklist2.Add(_track2);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist2);

            Assert.That(Tracksaver.ElementAt(0).CompassCourse, Is.EqualTo(90));
        }
        [Test]
        public void TestCalculateCompassCourse_compassIs63()
        {
            _tracklist1.Add(_track1);
            _tracklist2.Add(_track2);
            _tracklist3.Add(_CourseTrack);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist2);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist3);

            Assert.That(Math.Round(Tracksaver.ElementAt(0).CompassCourse), Is.EqualTo(63));
        }
        [Test]
        public void TestCalculateCompassCourse_compassIs0()
        {
            _tracklist1.Add(_track1);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);

            Assert.That(Tracksaver.ElementAt(0).CompassCourse, Is.EqualTo(0));
        }

        [Test]
        public void TestSeparationHandler_()
        {

        }

        [Test]
        public void TestLoggerRecievedList()
        {
            _tracklist1.Add(_track1);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);

            Assert.That(Tracksaver, Is.EqualTo(_tracklist1));
        }

        [Test]
        public void TestLoggerRecievedNothing()
        {
            
            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);

            Assert.That(Tracksaver, Is.EqualTo(null));
        }

        [Test]
        public void TestLoggerRecievedLastList()
        {
            _tracklist1.Add(_track1);
            _tracklist2.Add(_track2);

            _sut.TrackHandlerDataHandler += (sender, datas) => { Tracksaver = datas; };

            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist1);
            _fakeDecoder.DecodedDataHandler += Raise.Event<EventHandler<List<TrackData>>>(this, _tracklist2);

            Assert.That(Tracksaver, Is.EqualTo(_tracklist2));
        }
    }
}
