using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using AirTrafficController.Framework;
using AirTrafficController.Calculating;
using TransponderReceiver;

namespace AirTrafficController.Test.integration
{
    class IntegrationTest2
    {
        private ISeparationHandler _separationHandler;
        private CalculateCompassCourse _calculateCompassCource;
        private IDecoder _fakeDecoder;
        private CalculateVelocity _calcVelocity;
        private TrackHandler _TrackHandler;
        private Decoder _sut;
        private ITransponderReceiver _FakeITransponderReciever;

        

        private ICollection<TrackData> DataSaver;

        private TrackData _Result;
        private RawTransponderDataEventArgs _eventArgs;
        private RawTransponderDataEventArgs _eventArgs2;
        private List<string> _list1;
        private List<string> _list2;
        private List<string> _list3;


        [SetUp]
        public void Setup()
        {
            _separationHandler = new SeparationHandler();
            _calculateCompassCource = new CalculateCompassCourse();
            _calcVelocity = new CalculateVelocity();
            _FakeITransponderReciever = Substitute.For<ITransponderReceiver>();

            _sut = new Decoder(_FakeITransponderReciever);

            _list1 = new List<string>();
            _list2 = new List<string>();
            _list3 = new List<string>();
            _eventArgs = new RawTransponderDataEventArgs(_list1);
            _eventArgs2 = new RawTransponderDataEventArgs(_list2);

        }

        [Test]
        public void RecievedDataAndDecoded()
        {
            _list1.Add("AAAAA;90000;90000;1000;20180405202018");
            _Result = new TrackData
            {
                TagId = "AAAAA",
                X = 90000,
                Y = 90000,
                Altitude = 1000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 18)
            };

            _sut.DecodedDataHandler += (sender, datas) => { DataSaver = datas; };

            _FakeITransponderReciever.TransponderDataReady += Raise.EventWith(_eventArgs);

            Assert.That(DataSaver.ElementAt(0).Altitude, Is.EqualTo(_Result.Altitude));

        }
        [Test]
        public void RecievedData2TimesAndResultIsTheSecondData()
        {
            _list1.Add("AAAAA;90000;90000;1000;20180405202018");
            _list2.Add("AAAAA;90000;90000;2000;20180405202018");
            _Result = new TrackData
            {
                TagId = "AAAAA",
                X = 90000,
                Y = 90000,
                Altitude = 2000,
                TimeStamp = new DateTime(2018, 04, 05, 20, 20, 18)
            };

            _sut.DecodedDataHandler += (sender, datas) => { DataSaver = datas; };

            _FakeITransponderReciever.TransponderDataReady += Raise.EventWith(_eventArgs);
            _FakeITransponderReciever.TransponderDataReady += Raise.EventWith(_eventArgs2);

            Assert.That(DataSaver.ElementAt(0).Altitude, Is.EqualTo(_Result.Altitude));

        }
    }
}
