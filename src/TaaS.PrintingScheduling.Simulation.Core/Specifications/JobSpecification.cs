using System.Runtime.Serialization;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    [DataContract]
    public class JobSpecification<TTime> where TTime : struct
    {
        public JobSpecification(
            int id,
            double resolution,
            Dimension dimension,
            TTime incomingTime)
        {
            Id = id;
            Resolution = resolution;
            Dimension = dimension;
            IncomingTime = incomingTime;
        }
        
        public int Id { get; }

        public double Resolution { get; }

        public Dimension Dimension { get; }

        public TTime IncomingTime { get; }
    }
}