using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficController
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
