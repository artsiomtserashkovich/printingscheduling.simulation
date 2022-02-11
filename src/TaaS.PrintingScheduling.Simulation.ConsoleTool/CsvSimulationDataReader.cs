using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using CsvHelper;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.ConsoleTool
{
    public class CsvSimulationDataReader
    {
        public IReadOnlyCollection<JobSpecification<long>> ReadJobs(string csvFilePath)
        {
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<CsvJobRecord>()
                .Select(record => new JobSpecification<long>(
                    record.id,
                    record.resolution,
                    new Dimension(record.xdim, record.ydim, record.zdim),
                    record.incoming,
                    record.coef))
                .ToArray();
        }
        
        public IReadOnlyCollection<PrinterSpecification> ReadPrinters(string csvFilePath)
        {
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<CsvPrinterRecord>()
                .Select(record => new PrinterSpecification(
                    record.id,
                    record.speed,
                    new Dimension(record.xdim, record.ydim, record.zdim),
                    record.resolution))
                .ToArray();
        }
        
        
        private class CsvPrinterRecord
        {
            public int id { get; set; }

            public double speed { get; set; }
            
            public double resolution { get; set; }

            public double xdim { get; set;}
        
            public double ydim { get; set;}
        
            public double zdim { get; set;}
        }
        
        private class CsvJobRecord
        {
            public int id { get; set; }

            public double resolution { get; set; }

            public double xdim { get; set;}
        
            public double ydim { get; set;}
        
            public double zdim { get; set;}

            public long incoming { get; set;}
            
            public double coef { get; set;}
        }
    }
}