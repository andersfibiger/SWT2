using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
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
            _uut = new Logger();
        }

        [Test]
        public void LoggerOutput_TestIfPrintWasSucces()
        {
            string[] printData = new string[] { "ABC123", "15000", "15000", "10000", "20151006213456789" };

            //test if print (Logger) was succes
            Assert.IsTrue(_uut.LogData(printData));
        }

        [Test]
        public void LoggerOutput_TestIfPrintReturnsFalseWithNoData()
        {
            string[] printData = new string[] { };

            //test if print (Logger) was false
            Assert.IsFalse(_uut.LogData(printData));
        }
    }
}
