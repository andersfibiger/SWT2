using System;
using System.Collections.Generic;

namespace AirTrafficController.Framework
{
    public interface ISeparationHandler
    {
        List<string> CheckForSeparationEvents(List<string[]> trackList);
    }
}