using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficController
{
    public class CalculateVelocity
    {
        public void CalcVelocity(TrackData OldData, TrackData NewData)
        {
            double XYDiff = Math.Sqrt((Math.Pow((NewData.X - OldData.X), 2) + Math.Pow((NewData.Y - OldData.Y), 2)));
            double AltitudeDiff = Math.Abs(OldData.Altitude - NewData.Altitude);
            double DistanceMoved = Math.Sqrt((Math.Pow(XYDiff, 2) + Math.Pow(AltitudeDiff, 2)));
            double TimeUsed = NewData.TimeStamp.Subtract(OldData.TimeStamp).TotalSeconds;
            NewData.Velocity = (DistanceMoved / TimeUsed);
        }
    }
}
