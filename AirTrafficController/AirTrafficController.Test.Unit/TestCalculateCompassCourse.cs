using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Calculating;
using AirTrafficController.Framework;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestCalculateCompassCourse
    {

        TrackData fakeOldData = new TrackData() { X = 15000, Y = 15000 };
        TrackData fakeNewData = new TrackData() { X = 20000, Y = 20000 };
        CalculateCompassCourse testCalc = new CalculateCompassCourse(); 

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CalculateCompass_TestIfCalculationIsCorrect()
        {
            testCalc.CalcCompassCourse(fakeOldData, fakeNewData);
            Assert.AreEqual(225, fakeNewData.CompassCourse);
        }
    }
}
