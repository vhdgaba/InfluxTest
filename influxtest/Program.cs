using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using Task = System.Threading.Tasks.Task;

namespace influxtest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            InfluxWriter.InitializeClient();

            InfluxWriter.InfluxWrite();

            await InfluxWriter.InfluxRead();

            List<InfluxPair> pairs = InfluxWriter.GetResults();
            pairs.ForEach(pair =>
            {
                Console.WriteLine($"{pair.Time}: {pair.Value}");
            });

            Console.WriteLine("end");
            Console.ReadKey();


            InfluxWriter.DisposeClient();
        }
        
    }
}
