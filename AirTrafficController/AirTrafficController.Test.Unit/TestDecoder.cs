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
    class TestDecoder
    {
        private IDecoder _uut;
        private List<TrackData> _decodedData;

        [SetUp]
        public void Setup()
        {
            ITransponderReceiver tr = Substitute.For<ITransponderReceiver>();
            _uut = new Decoder(tr);
            _decodedData = new List<TrackData>();
            _uut.DecodedDataHandler += DecodedDataHandler;
        }

        private void DecodedDataHandler(object sender, List<TrackData> e)
        {
            _decodedData = e;
        }

        [Test]
        public void CheckIf_TestData_HasCountEqualTo_IsTrue()
        {
            //Hardcoreded testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            //Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Assert that the decoder 
            _uut.DecodeData(null, RawtestData);
            Assert.That(_decodedData, Has.Count.EqualTo(1)); // Test that the decode only has decoded the one string array
            //Assert.That(DecodedData[0].Count, Is.EqualTo(5)); // Test that the string array has the correct amount decoded.
        }
        [Test]
        public void DecodeData_TestIfDataIsCorrect_IsTrue()
        {
            //Hardcore testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            // Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Hardcode the correct decoded data

            TrackData CorrectTestData = new TrackData()
            {
                TagId = "ATR423",
                X = 39045,
                Y = 12932,
                Altitude = 14000,
                TimeStamp = DateTime.ParseExact("20151006213456789",
                    "yyyyMMddHHmmssfff",
                    null)
            };
            //Assert that the test data has been read.
            _uut.DecodeData(null, RawtestData);
            foreach (var trackData in _decodedData)
            {
                Assert.That(trackData.TagId, Is.EqualTo(CorrectTestData.TagId));
                Assert.That(trackData.X, Is.EqualTo(CorrectTestData.X));
                Assert.That(trackData.Y, Is.EqualTo(CorrectTestData.Y));
                Assert.That(trackData.Altitude, Is.EqualTo(CorrectTestData.Altitude));
                Assert.That(DateTime.Compare(trackData.TimeStamp, CorrectTestData.TimeStamp) , Is.Zero);
            }
        }

    }
}
