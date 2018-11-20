using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Calculating;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestCalculateVelocity
    {
        TrackData fakeOldData = new TrackData() { X = 15000, Y = 15000, Altitude = 10000, TimeStamp = DateTime.ParseExact("20151006213456000",
            "yyyyMMddHHmmssfff",
            null)
        };

        private TrackData fakeNewData = new TrackData()
        {
            X = 15000,
            Y = 16000,
            Altitude = 10000,
            TimeStamp = DateTime.ParseExact("20151006213459000",
                "yyyyMMddHHmmssfff",
                null)
        };
        CalculateVelocity testCalc = new CalculateVelocity();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CalculateVelocity_TestIfCalculationIsCorrect()
        {
            testCalc.CalcVelocity(fakeOldData, fakeNewData);
            Assert.AreEqual(333.33333333333331d, fakeNewData.Velocity);
        }
    }
}
