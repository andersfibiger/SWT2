using System;

namespace AirTrafficController
{
    public class TrackData
    {
        public string TagId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}