using System;
using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController
{
    public class SeparationHandler : ISeparationHandler
    {
        private const double MinDistance = 5000.0; // in meters
        private const int MinAltitude = 300;
        public List<string> CheckForSeparationEvents(List<string[]> trackList)
        {
            List<string> separationEventList = new List<string>();
            // The ugly O((n+1)/2) implementation of the comparison
            for (int i = 0; i < trackList.Count; i++)
            {
                string[] track1StringArray = trackList[i];
                Int32.TryParse(track1StringArray[1], out var currentX1);
                Int32.TryParse(track1StringArray[2], out var currentY1);
                Int32.TryParse(track1StringArray[3], out var currentZ1);
                // Compare with the rest of the tracks
                for (int j = i + 1; j < trackList.Count; j++)
                {
                    string[] track2StringArray = trackList[j];
                    Int32.TryParse(track2StringArray[1], out var currentX2);
                    Int32.TryParse(track2StringArray[2], out var currentY2);
                    Int32.TryParse(track2StringArray[3], out var currentZ2);

                    // Calculate the distance with Pythagoras' theorem.
                    if (Math.Abs(currentZ1 - currentZ2) < MinAltitude && 
                        Math.Sqrt(Math.Pow(currentX1 - currentX2, 2) + Math.Pow(currentY1 - currentY2, 2)) < MinDistance)
                    {
                        // Separation event occured! Save the time of the occurence and both track's tags.
                        separationEventList.Add($"{track1StringArray[4]};{track1StringArray[0]};{track2StringArray[0]}");
                    }
                }
            }
            return separationEventList;
        }
    }
}