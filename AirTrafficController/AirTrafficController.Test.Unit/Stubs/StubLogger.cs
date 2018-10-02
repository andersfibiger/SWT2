using AirTrafficController.Framework;

namespace AirTrafficController.Test.Unit.Stubs
{
    public class StubLogger : ILogger
    {
        public void LogData(string[] dataTracks)
        {
            throw new System.NotImplementedException();
        }
    }
}