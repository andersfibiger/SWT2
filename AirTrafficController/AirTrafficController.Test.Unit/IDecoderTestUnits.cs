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

        private static readonly List<string[]> ResultData1 = new List<string[]>()
        {
            new string[] {"ATR423", "39045", "12932", "14000", "20151006213456789"},
            //new string[] { "BCD123", "10005", "85890", "12000", "20151006213456789"},
            //new string[] {"TESTBOUNDX","5000","0","100","2018"},
        };
        [Test]
        public void CheckIf_TestData_HasCountEqualTo_isTrue()
        {
            //Hardcoreded testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            //testData.Add("BCD123;10005;85890;12000;20151006213456789");
            //testData.Add("XYZ987;25059;75654;4000;20151006213456789");
            //Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Assert that the decoder 
            List<string[]> DecodedData = _uut.DecodeData(RawtestData);
            Assert.That(DecodedData, Is.Not.Empty); // Test that the decoder has decoding something
            Assert.That(DecodedData, Has.Count.EqualTo(1)); // Test that the decode only has decoded the one string array
            Assert.That(DecodedData[0].Count, Is.EqualTo(5)); // Test that the string array has the correct amount decoded.
        }
        [Test]
        public void DecodeData_TestIfDataIsCorrect_IsTrue()
        {
            //Hardcore testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            //testData.Add("BCD123;10005;85890;12000;20151006213456789");
            //testData.Add("XYZ987;25059;75654;4000;20151006213456789");
            // Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Hardcorde the correct decoded data
            string[] CorrectTestData = {"ATR423", "39045", "12932", "14000", "20151006213456789" };
            //Assert that the test data has been read.
            List<string[]> DecodedData = _uut.DecodeData(RawtestData);
            // test the data in a for each
            for (int i = 0; i < DecodedData[0].Length; i++)
            {
                Console.WriteLine("Counting DecodeData...");
                string data = (string)DecodedData[0][i];
                //data = CorrectTestData[i];
                Assert.That(data, Is.EqualTo(CorrectTestData[i]));
            }
            //Assert.That())
        }

    }
}
