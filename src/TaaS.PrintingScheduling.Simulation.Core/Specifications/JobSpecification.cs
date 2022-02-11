using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    [DataContract]
    public class JobSpecification<TTime> where TTime : struct
    {
        public JobSpecification(
            int id,
            double resolution,
            Dimension dimension,
            TTime incomingTime,
            double priorityCoefficient)
        {
            Id = id;
            Resolution = resolution;
            Dimension = dimension;
            IncomingTime = incomingTime;
            PriorityCoefficient = priorityCoefficient;
        }
        
        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("resolution")]
        public double Resolution { get; }

        [JsonIgnore]
        public Dimension Dimension { get; }

        [JsonPropertyName("incoming")]
        public TTime IncomingTime { get; }

        [JsonPropertyName("coefficient")]
        public double PriorityCoefficient { get; }
    }
}