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
        public void DecodeData_TestIfDataIsCorrect_IsTrue()
        {
            //Hardcore testData
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");
            // Make new RawTransponderDataEventArgs
            RawTransponderDataEventArgs RawtestData = new RawTransponderDataEventArgs(testData);
            //Assert that the test data has been read.
             

        }

    }
}
