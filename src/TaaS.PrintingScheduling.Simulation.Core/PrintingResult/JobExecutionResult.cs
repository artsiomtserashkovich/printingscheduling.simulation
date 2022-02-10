using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.PrintingResult
{
    public class JobExecutionResult<TTime> where TTime : struct
    {
        public JobExecutionResult(
            JobSpecification<TTime> job, 
            PrinterSpecification printer,)
        
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
        
        [JsonPropertyName("jobId")]
        public int JobId { get; }
        
        [JsonPropertyName("printerId")]
        public int PrinterId { get; }
        
        [JsonPropertyName("incoming")]
        public TTime IncomingTime { get; }

        [JsonPropertyName("scheduleTimeSlot")]
        public TimeSlot<TTime> ScheduledTime { get; }
        
        [JsonPropertyName("executionTimeSlot")]
        public TimeSlot<TTime> ExecutionTime { get; }
    }
}