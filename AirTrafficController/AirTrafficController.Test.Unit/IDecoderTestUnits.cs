using System;
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
            List<string[]> DecodedData = _uut.DecodeData(RawtestData);
            Assert.That(DecodedData, Has.Count.EqualTo(1)); // Test that the decode only has decoded the one string array
            Assert.That(DecodedData[0].Count, Is.EqualTo(5)); // Test that the string array has the correct amount decoded.
        }
        [Test]
        public void DecodeData_TestIfDataIsCorrect_IsTrue()
        {
            //Hardcore testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            // Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Hardcorde the correct decoded data
            string[] CorrectTestData = {"ATR423", "39045", "12932", "14000", "20151006213456789" };
            //Assert that the test data has been read.
            List<string[]> DecodedData = _uut.DecodeData(RawtestData);
            // test the data in a for each
            for (int i = 0; i < DecodedData[0].Length; i++)
            {
                string data = (string)DecodedData[0][i];
                //data = CorrectTestData[i];
                Assert.That(data, Is.EqualTo(CorrectTestData[i]));
            }
        }

    }
}
