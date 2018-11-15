using System;

namespace AirTrafficController.Calculating
{
    public class CalculateCompassCourse
    {
        public void CalcCompassCourse(TrackData OldData, TrackData NewData)
        {
            double deltax = OldData.X - NewData.X;
            double deltay = OldData.Y - NewData.Y;

            double CompassCourse = Math.Atan2(deltay, deltax) * (180 / Math.PI);

            if (CompassCourse < 0)
            {
                CompassCourse += 360;
            }

            NewData.CompassCourse = CompassCourse;
        }
    }
}
