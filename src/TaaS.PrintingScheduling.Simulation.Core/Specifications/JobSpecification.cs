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
        
        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "resolution")]
        public double Resolution { get; }
        
        [DataMember(Name = "volume")]
        public double Volume { get; }
        
        [DataMember(Name = "dimension")]
        public Dimension Dimension { get; }

        [DataMember(Name = "incoming")]
        public TTime IncomingTime { get; }
    }
}