/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NUnit.Framework;
using TransponderReceiver;

namespace AirTrafficController.Test.Unit
{
    class IDecoderTestUnits
    {
        private IDecoder _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new Decoder();
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
            List<TrackData> DecodedData = _uut.DecodeData(RawtestData);
            Assert.That(DecodedData, Has.Count.EqualTo(1)); // Test that the decode only has decoded the one string array
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
            foreach (var trackData in _uut.DecodeData(RawtestData))
            {
                Assert.That(trackData.TagId, Is.EqualTo(CorrectTestData.TagId));
                Assert.That(trackData.X, Is.EqualTo(CorrectTestData.X));
                Assert.That(trackData.Y, Is.EqualTo(CorrectTestData.Y));
                Assert.That(trackData.Altitude, Is.EqualTo(CorrectTestData.Altitude));
                Assert.That(DateTime.Compare(trackData.TimeStamp, CorrectTestData.TimeStamp) , Is.Zero);
            }
        }

    }
}*/
