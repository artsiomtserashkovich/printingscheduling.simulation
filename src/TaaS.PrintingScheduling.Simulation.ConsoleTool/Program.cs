using System;
using TaaS.PrintingScheduling.Simulation.Core;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders;

namespace TaaS.PrintingScheduling.Simulation.ConsoleTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new CsvSimulationDataReader();

            var printers = reader.ReadPrinters("B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\data\\case_1\\printers.csv");
            var jobs = reader.ReadJobs("B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\data\\case_1\\jobs.csv");
            
            new CycleSimulationEngineBuilder()
                .WithPrinters(printers)
                .WithPrintingSystem(builder => builder
                    .WithLeastFinishTimeScheduler()
                    .WithIncomingJobs(jobs))
                
                .Build()
                    .Simulate();
        }
    }
    
    
    
    /*var printers =
        new[]
        {
            new PrinterSpecification(1, 20, new Dimension(300, 300, 300), 0.3),
            new PrinterSpecification(2, 30, new Dimension(200, 200, 200), 0.4),
            new PrinterSpecification(3, 25, new Dimension(100, 100, 100), 0.5),
            new PrinterSpecification(4, 30, new Dimension(250, 250, 250), 0.4),
            new PrinterSpecification(5, 30, new Dimension(150, 150, 150), 0.2),
            new PrinterSpecification(6, 20, new Dimension(100, 100, 100), 0.6),
        };
    var jobs =
        new[]
        {
            new JobSpecification<long>(1, 0.4, new Dimension(80, 100, 50), 1),
            new JobSpecification<long>(2, 0.3, new Dimension(80, 100, 50), 2),
            new JobSpecification<long>(3, 0.5, new Dimension(80, 60, 30), 3),
            new JobSpecification<long>(4, 0.6, new Dimension(80, 100, 50), 4),
            new JobSpecification<long>(5, 0.3, new Dimension(80, 100, 50), 5),
            new JobSpecification<long>(6, 0.5, new Dimension(80, 100, 50), 6),
            new JobSpecification<long>(7, 0.2, new Dimension(80, 100, 50), 7),
            new JobSpecification<long>(8, 0.4, new Dimension(80, 100, 50), 8),
            new JobSpecification<long>(9, 0.7, new Dimension(80, 100, 50), 9),
            new JobSpecification<long>(10, 0.3, new Dimension(80, 100, 50), 10),
            new JobSpecification<long>(11, 0.4, new Dimension(80, 100, 50), 11),
            new JobSpecification<long>(12, 0.5, new Dimension(80, 100, 50), 12),
        };*/
}