using System;
using System.Collections.Generic;
using TransponderReceiver;

namespace AirTrafficController.Framework
{
    public interface IDecoder
    {
        void DecodeData(object sender, RawTransponderDataEventArgs data);
        event EventHandler<List<TrackData>> DecodedDataHandler;
    }
}