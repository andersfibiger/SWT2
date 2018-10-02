using System.Collections.Generic;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController.Test.Unit.Stubs
{
    public class StubDecoder : IDecoder
    {
        public List<string[]> DecodeData(RawTransponderDataEventArgs data)
        {
            throw new System.NotImplementedException();
        }
    }
}