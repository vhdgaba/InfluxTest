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
    class InfluxWriter
    {
        private static InfluxDBClient influxDBClient = null;
        private static readonly char[] Token = "teRoDg1JyBrV6VCuVgXA0RfNVg2M2V1H0ViqxLvRDh1p5-oqX2qxQdDjr_SPyEWvrCjUYMM1HilSADqTE0Vetw==".ToCharArray();
        private static List<InfluxPair> pairs = null;

        private InfluxWriter()
        {
        }

        public static void InitializeClient()
        {
            if (influxDBClient == null)
            {
                influxDBClient = InfluxDBClientFactory.Create("http://localhost:4081", Token);
            }
        }

        public static void InfluxWrite()
        {
            using (var writeApi = influxDBClient.GetWriteApi())
            {
                writeApi.WriteRecord("vhgbucket", "vhg", WritePrecision.Ns, "temperature,location=home value=66");
            }
        }

        public static async Task InfluxRead()
        {
            if (pairs == null)
            {
                pairs = new List<InfluxPair>();
            }
            else
            {
                pairs.Clear();
            }

            var flux = "from(bucket:\"vhgbucket\") |> range(start: -4h)";

            var fluxTables = await influxDBClient.GetQueryApi().QueryAsync(flux, "vhg");
            fluxTables.ForEach(fluxTable =>
            {
                var fluxRecords = fluxTable.Records;
                fluxRecords.ForEach(fluxRecord =>
                {
                    pairs.Add(new InfluxPair() { Time = fluxRecord.GetTime().ToString(), Value = fluxRecord.GetValue().ToString() });
                    //Console.WriteLine($"{fluxRecord.GetTime()}: {fluxRecord.GetValue()}");
                });
            });
        }

        public static List<InfluxPair> GetResults()
        {
            return pairs;
        }

        public static void DisposeClient()
        {
            influxDBClient.Dispose();
        }
    }

    class InfluxPair {
        public string Time { get; set; }
        public string Value { get; set; }
    }
}