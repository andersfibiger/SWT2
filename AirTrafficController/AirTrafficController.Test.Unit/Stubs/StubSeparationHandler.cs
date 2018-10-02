using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController.Test.Unit.Stubs
{
    public class StubSeparationHandler : ISeparationHandler
    {
        public List<string> CheckForSeparationEvents(List<string> trackList)
        {
            throw new System.NotImplementedException();
        }
    }
}