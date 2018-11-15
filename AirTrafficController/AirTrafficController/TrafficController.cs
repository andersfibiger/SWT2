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
        private readonly ITrackHandler _track;
        public int numberOfTracks = 0;

        public TrafficController(
            IDecoder decoder, 
            ILogger logger, 
            ITrackHandler track, 
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
            List<TrackData> trackDataList = _decoder.DecodeData(e);
            //keep track of flights in Airspace. Great for testing
            _track.UpdateTracks(trackDataList);
            numberOfTracks = trackDataList.Count;
            // TODO Not implemented yet!
            // _track.GetListOfSeparationEvents();
            _logger.ClearData();
            // We iterate over every aircraft.
            foreach (TrackData trackData in trackDataList)
            {
                _logger.LogData(trackData);
            }


        }


    }
}