using System;
using System.Collections.Generic;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController
{
    public class TrafficController
    {
        private readonly IDecoder _decoder;
        private readonly ILogger _logger;
        private readonly ITrack _track;

        public TrafficController(
            IDecoder decoder, 
            ILogger logger, 
            ITrack track, 
            ITransponderReceiver transponderReceiver)
        {
            // Subscribe to transponder events with a custom callback method.
            transponderReceiver.TransponderDataReady += TransReceiverOnTransponderDataReady;

            _decoder = decoder;
            _logger = logger;
            _track = track;
        }

        private void TransReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e)
        {
            List<string[]> tracksStringList = _decoder.DecodeData(e);
            _track.UpdateTracks(tracksStringList);
            // TODO Not implemented yet!
            // _track.GetSeparationEventsList();
            Console.Clear();
            // We iterate over every aircraft.
            foreach (string[] trackData in tracksStringList)
            {
                _logger.LogData(trackData);
            }
        }


    }
}