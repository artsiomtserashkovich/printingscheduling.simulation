using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs
{
    public interface ICycledJob
    {
        JobSpecification<long> Specification { get; }
        
        bool IsComplete { get; }

        void Execute(PrinterSpecification printer, ICycledSimulationContext simulationContext);
    }
}