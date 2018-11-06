using System;
using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class SeparationHandler : ISeparationHandler
    {
        private const double MinDistance = 5000.0; // in meters
        private const int MinAltitude = 300;
        public List<string> CheckForSeparationEvents(List<TrackData> trackList)
        {
            List<string> separationEventList = new List<string>();
            // The ugly O((n+1)/2) implementation of the comparison
            for (int i = 0; i < trackList.Count; i++)
            {
                TrackData trackData1 = trackList[i];
                // Compare with the rest of the tracks
                for (int j = i + 1; j < trackList.Count; j++)
                {
                    TrackData trackData2 = trackList[j];
                    // Calculate the distance with Pythagoras' theorem.
                    if (Math.Abs(trackData1.Altitude - trackData2.Altitude) < MinAltitude && 
                        Math.Sqrt(Math.Pow(trackData1.X - trackData2.X, 2) + Math.Pow(trackData1.Y - trackData2.Y, 2)) < MinDistance)
                    {
                        // Separation event occured! Save the time of the occurence and both track's tags.
                        separationEventList.Add($"{trackData1.TimeStamp};{trackData1.TagId};{trackData2.TagId}");
                    }
                }
            }
            return separationEventList;
        }
    }
}