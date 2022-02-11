using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    public readonly struct TimeSlot<TTime> where TTime : struct
    {
        [JsonPropertyName("start")]
        public TTime Start { get; }
        
        [JsonPropertyName("finish")]
        public TTime Finish { get; }

        public TimeSlot(TTime start, TTime finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}