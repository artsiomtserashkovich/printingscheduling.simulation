using System;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders;

namespace TaaS.PrintingScheduling.Simulation.ConsoleTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new CsvSimulationDataReader();

            var printers = reader.ReadPrinters("B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\data\\case_2(incomingv2)\\printers.csv");
            var jobs = reader.ReadJobs("B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\data\\case_2(incomingv2)\\jobs.csv");
            
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
                    $"finishJob:'{result.JobId}'; " +
                    $"printer:'{result.PrinterId}'; " +
                    $"incoming:'{result.IncomingTime}'; " +
                    $"executionStartTime:'{result.ExecutionTime.Start}'; " +
                    $"executionFinishTime:'{result.ExecutionTime.Finish}'; " +
                    $"scheduledStartTime:'{result.ScheduledTime.Start}'; " +
                    $"scheduledFinishTime:'{result.ScheduledTime.Finish}'.");
                
            }
            var json = System.Text.Json.JsonSerializer.Serialize(results);
            Console.WriteLine(json);
        }
    }
}