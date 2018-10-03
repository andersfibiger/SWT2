using System;
using System.Collections.Generic;
using TransponderReceiver;

namespace AirTrafficController.Test.Unit.Stubs
{
    public class StubTransponderReceiver : TransponderReceiver.ITransponderReceiver
    {
        private static List<string> fakeDataList = new List<string>(
            new string[]
            {
                "AAA111;630094;83421;12500;20181002204132530",
                "BBB111;930094;3421;3000;20181002204132520",
                "C1C1C1;9094;43210;500;201810022047872530"
            });
        public RawTransponderDataEventArgs eventArgs = new RawTransponderDataEventArgs(fakeDataList);

        public event EventHandler<RawTransponderDataEventArgs> TransponderDataReady;
        
    }
}