using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor
{
    public interface IPrintingJobExecutable<TTime> where TTime : struct
    {
        public JobSpecification<TTime> Specification { get; }

        public PrintingJobIterationResult ExecuteIteration(PrinterSpecification printer, TTime executionTime);
    }

    public abstract class PrintingJobIterationResult
    {
        protected PrintingJobIterationResult(bool isComplete)
        {
            IsComplete = isComplete;
        }
        
        public bool IsComplete { get; }
    }
    
    public class NotCompletePrintingIterationResult : PrintingJobIterationResult
    {
        public double RemainingPrintingVolume { get; }

        public NotCompletePrintingIterationResult(double remainingPrintingVolume) : base(false)
        {
            RemainingPrintingVolume = remainingPrintingVolume;
        }
    }
    
    public class CompletedPrintingIterationResult<TTime> : PrintingJobIterationResult where TTime : struct
    {
        public TimeSlot<TTime> ScheduledTime { get; }
        
        public TimeSlot<TTime> ExecutionTime { get; }

        public CompletedPrintingIterationResult(TimeSlot<TTime> scheduledTime, TimeSlot<TTime> executionTime) : base(true)
        {
            ScheduledTime = scheduledTime;
            ExecutionTime = executionTime;
        }
    }
}