using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs
{
    public interface ICycledJob
    {
        public JobSpecification<long> Specification { get; }
        
        public bool IsComplete { get; }

        public JobExecutionResult<long> GetResultReport();

        public void Execute(PrinterSpecification printer, ICycledSimulationContext simulationContext);
    }
}