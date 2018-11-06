using System.Collections.Generic;
using TransponderReceiver;

namespace AirTrafficController.Framework
{
    public interface IDecoder
    {
        List<TrackData> DecodeData(RawTransponderDataEventArgs data);
    }
}