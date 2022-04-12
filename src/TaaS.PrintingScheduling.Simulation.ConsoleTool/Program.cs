using System;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders;
using TaaS.PrintingScheduling.Simulation.WalltimeDashboard;

namespace TaaS.PrintingScheduling.Simulation.ConsoleTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new CsvSimulationDataReader();

            var folderPath = "B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\data\\case_5\\";
            
            var printers = reader.ReadPrinters(folderPath + "printers.csv");
            var jobs = reader.ReadJobs(folderPath + "jobs.csv");
            
            var results = new CycleSimulationEngineBuilder()
                .WithPrinters(printers)
                .WithPrintingSystem(builder => builder
                    .WithLeastFinishTimeScheduler()
                    .WithIncomingJobs(jobs))
                .Build()
                    .Simulate();

            foreach (var result in results)
            {
                Console.WriteLine(
                    $"finishJob:'{result.Job.Id}'; " +
                    $"printer:'{result.Printer.Id}'; " +
                    $"incoming:'{result.Job.Dimension}'; " +
                    $"executionStartTime:'{result.ExecutionTime.Start}'; " +
                    $"executionFinishTime:'{result.ExecutionTime.Finish}'; " +
                    $"scheduledStartTime:'{result.ScheduledTime.Start}'; " +
                    $"scheduledFinishTime:'{result.ScheduledTime.Finish}'.");
                
            }
            
            new ReportGenerator<long>().Generate(results, folderPath, "least");
        }
    }
}