using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.PrintingResult
{
    public class JobExecutionResult<TTime> where TTime : struct
    {
        public JobExecutionResult(
            int jobId, 
            int printerId, 
            TTime incomingTime, 
            TimeSlot<TTime> scheduledTime, 
            TimeSlot<TTime> executionTime)
        {
            JobId = jobId;
            PrinterId = printerId;
            IncomingTime = incomingTime;
            ScheduledTime = scheduledTime;
            ExecutionTime = executionTime;
        }
        
        public int JobId { get; }
        
        public int PrinterId { get; }
        
        public TTime IncomingTime { get; }

        public TimeSlot<TTime> ScheduledTime { get; }
        
        public TimeSlot<TTime> ExecutionTime { get; }
    }
}