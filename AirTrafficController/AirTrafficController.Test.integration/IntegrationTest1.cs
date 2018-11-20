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
        private ISeparationHandler _fakeSeparationHandler;
        private CalculateCompassCourse _fakeCalculateCompassCource;
        private IDecoder _fakeDecoder;
        private CalculateVelocity calcVelocity;
        private TrackHandler trackHandler;
        private TrackData _track1;
        private TrackData _track2;
        private List<TrackData> _tracklist1;
        private List<TrackData> _tracklist2;

        [SetUp]
        public void Setup()
        {
            _fakeSeparationHandler = Substitute.For<ISeparationHandler>();
            _fakeCalculateCompassCource = Substitute.For<CalculateCompassCourse>();
            _fakeDecoder = Substitute.For<IDecoder>();

            trackHandler = new TrackHandler(_fakeSeparationHandler,calcVelocity,_fakeCalculateCompassCource,_fakeDecoder);
            _tracklist1 = new List<TrackData>();
            _tracklist2 = new List<TrackData>();


            _track1 = new TrackData
            {
                TagId = "AAAAA",
                X = 90000,
                Y = 90000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 18)
            };
            _track2 = new TrackData
            {
                TagId = "AAAAA",
                X = 90000,
                Y = 80000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 20)
            };
        }


        [Test]
        public void TestCalculateVelocityAndCompassCourse()
        {
            
            _tracklist1.Add(_track1);
            //trackHandler.oldData = _tracklist1;
            _tracklist2.Add(_track2);

            trackHandler.CalculateVelocityAndCompassCourse(_tracklist2);

            Assert.That(Math.Round(_track2.Velocity), Is.EqualTo(5000));

        }
    }
}
