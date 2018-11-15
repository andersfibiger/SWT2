using System;
using System.Collections.Generic;
using System.Globalization;
using AirTrafficController.Framework;
using TransponderReceiver;

namespace AirTrafficController
{
    public class Decoder : IDecoder
    {
        public event EventHandler<List<TrackData>> DecodedDataHandler;

        public void DecodeData(object sender, RawTransponderDataEventArgs data)
        {
            List<TrackData> formattedDataList = new List<TrackData>();
            // We iterate over every aircraft.
            foreach (string airCraftsData in data.TransponderData)
            {
                // For each aircraft, split the string into separate data items. 
                var trackItems = airCraftsData.Split(';');
                Int32.TryParse(trackItems[1], out var coordinateX);
                Int32.TryParse(trackItems[2], out var coordinateY);
                Int32.TryParse(trackItems[3], out var altitude);
                DateTime dateTime;
                dateTime = DateTime.TryParseExact(trackItems[4],
                    "yyyyMMddHHmmssfff",
                    null,
                    DateTimeStyles.None,
                    out dateTime)
                    ? dateTime
                    : DateTime.MinValue;

                formattedDataList.Add(new TrackData()
                {
                    TagId = trackItems[0],
                    X = coordinateX,
                    Y = coordinateY,
                    Altitude = altitude,
                    TimeStamp = dateTime
                });
            }

            DecodedDataHandler.Invoke(this, formattedDataList);
        }
    }
}