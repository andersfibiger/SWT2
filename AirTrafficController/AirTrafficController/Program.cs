using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace AirTrafficController
{
    class Program
    {
        static void Main(string[] args)
        {
           
                var list = new List<string>();

                var factory = TransponderReceiverFactory.CreateTransponderDataReceiver();
                var raw = new RawTransponderDataEventArgs(list);
            factory.TransponderDataRea
            while (true)
            {
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Press key");
            Console.ReadKey();
        }
    }
}
