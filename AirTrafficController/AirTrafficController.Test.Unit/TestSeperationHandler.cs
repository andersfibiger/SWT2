using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using NUnit.Framework;

namespace AirTrafficController.Test.Unit
{
    class TestSeperationHandler
    {
        private static ISeparationHandler _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new SeparationHandler(); 
        }

        private static readonly List<string[]> TracksNoSeparationList1 = new List<string[]>()
        {
            new string[] {"TESTAAA","0","0","100","2018"},
            new string[] {"TESTBBB","1256","62124","600","2018"},
            new string[] {"TESTBOUNDX","5000","0","100","2018"},
            new string[] {"TESTBOUNDYZ","0","5000","400","2018"},
            new string[] {"TESTBOUNDXYZ","4999","100","400","2018"}
        };

        private static readonly List<string[]> TracksNoSeparationList2 = new List<string[]>()
        {
            new string[] {"TESTAAA", "0", "0", "100", "2018"},
            new string[] {"TESTBOUNDXY","4999","100","100","2018"}
        };

        private static readonly List<string[]> TracksNoSeparationList3 = new List<string[]>()
        {
            new string[] {"TESTAAA","0","0","100","2018"},
            new string[] {"TESTBOUNDZ","6000","0","400","2018"}
        };

        private static readonly List<string[]> TracksNoSeparationList4 = new List<string[]>()
        {
            new string[] {"TESTAAA","0","0","100","2018"},
            new string[] {"TESTBOUNDXZ","5000","0","400","2018"}
        };

        // For good measure, test with negative coordinates as well
        private static readonly List<string[]> TracksNoSeparationList5 = new List<string[]>()
        {
            new string[] {"TESTAAA","0","0","100","2018"},
            new string[] {"TESTBOUNDXNEG","-5000","0","100","2018"},
            new string[] {"TESTBOUNDYNEG","0","-5000","100","2018"}
        };

        private static readonly List<string[]> TracksWithSeparationList1 = new List<string[]>()
        {
            new string[] {"TESTAAA", "0", "0", "100", "2018"},
            new string[] {"TESTBOUNDX", "4999", "0", "99", "2018"},
            new string[] {"TESTBOUNDY", "0", "4999", "99", "2018"},
            new string[] {"TESTBOUNDZ", "0", "4999", "399", "2018"},
            new string[] {"TESTBOUNDXNEG", "-4999", "0", "99", "2018"}
        };

        [Test]
        public static void SeparationEvents_ValuesInRange_ResultIsTrue()
        {
            Assert.That(_uut.CheckForSeparationEvents(TracksNoSeparationList1), Is.Empty);
            Assert.That(_uut.CheckForSeparationEvents(TracksNoSeparationList2), Is.Empty);
            Assert.That(_uut.CheckForSeparationEvents(TracksNoSeparationList3), Is.Empty);
            Assert.That(_uut.CheckForSeparationEvents(TracksNoSeparationList4), Is.Empty);
            Assert.That(_uut.CheckForSeparationEvents(TracksNoSeparationList5), Is.Empty);
        }

        [Test]
        public static void CheckForSeparationEvents_ValuesOutOfRange_ResultIsFalse()
        {
            Assert.That(_uut.CheckForSeparationEvents(TracksWithSeparationList1), Has.Count.EqualTo(4));
        }
    }
}
