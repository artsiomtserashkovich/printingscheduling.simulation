using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Jobs
{
    public interface ICycledJob
    {
        public JobSpecification<long> Specification { get; }
        
        public bool IsComplete { get; }

        public void Execute(PrinterSpecification printer);
    }
}