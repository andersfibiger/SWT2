using System.Collections.Generic;
using AirTrafficController.Framework;

namespace AirTrafficController.Test.Unit.Stubs
{
    public class StubTrack : ITrack
    {
        private readonly StubSeparationHandler stubSeparationHandler;

        public StubTrack(StubSeparationHandler stubSeparationHandler)
        {
            this.stubSeparationHandler = stubSeparationHandler;
        }

        public void UpdateTracks(List<string[]> trackList)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckIfWithinBoundary(int positionX, int positionY, int altitude)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetSeparationEventsList()
        {
            throw new System.NotImplementedException();
        }
    }
}