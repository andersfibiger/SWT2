﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    public class AirTrafficControllerTestUnits
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase(10000,10000,500)]
        [TestCase(90000, 90000, 20000)]

        public static void WithinBoundary_ValuesInRange_resultIsCorret(int x, int y, int a)
        {
            Assert.That(Program.checkIfWithinBoundary(x,y,a),Is.True);
        }

        //checking for just out of range on the lower and upper boundary for x
        [TestCase(9999, 15000, 1000)]
        [TestCase(90001, 15000, 1000)]

        public static void WithInBoundary_XoutOfRange_ResultIsFalse(int x, int y, int a)
        {
            Assert.That(Program.checkIfWithinBoundary(x,y,a), Is.False);
        }
    }
}
