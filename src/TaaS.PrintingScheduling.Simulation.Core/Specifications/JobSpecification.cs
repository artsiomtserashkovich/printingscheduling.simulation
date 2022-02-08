using System.Runtime.Serialization;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    [DataContract]
    public class JobSpecification<TTime> where TTime : struct
    {
        public JobSpecification(
            int id,
            double resolution, 
            double volume, 
            Dimension dimension,
            TTime incomingTime)
        {
            Id = id;
            Resolution = resolution;
            Volume = volume;
            Dimension = dimension;
            IncomingTime = incomingTime;
        }
        
        public int Id { get; }

        public double Resolution { get; }
        
        public double Volume { get; }
        
        public Dimension Dimension { get; }

        public TTime IncomingTime { get; }
    }
}