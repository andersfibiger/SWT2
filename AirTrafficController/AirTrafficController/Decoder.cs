using System;
using System.Collections.Generic;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController
{
    public class Decoder : IDecoder
    {
        public List<string[]> DecodeData(RawTransponderDataEventArgs data)
        {
            List<string[]> formattedDataList = new List<string[]>();
            // We iterate over every aircraft.
            foreach (string airCraftsData in data.TransponderData)
            {
                // For each aircraft, split the string into separate data items. 
                formattedDataList.Add(airCraftsData.Split(';'));
            }
            return formattedDataList;
        }
    }
}