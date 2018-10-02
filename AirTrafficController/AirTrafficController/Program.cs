using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController
{
    public class Program
    {
        private static readonly IDecoder _decoder = new Decoder();
        private static readonly ILogger _logger = new Logger();
        private static readonly ITrack _track = new Track(new SeparationHandler());

        static void Main(string[] args)
        {
            // Initiate a virtual airspace with air crafts and return a transponder receiver interface to it.
            var transReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            // Subscribe to transponder events with a custom callback method.
            transReceiver.TransponderDataReady += TransReceiverOnTransponderDataReady;

            while (true)
            {
                System.Threading.Thread.Sleep(1000); // Ugly way of saving some cycles
            }
        }

        private static void TransReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e)
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
    