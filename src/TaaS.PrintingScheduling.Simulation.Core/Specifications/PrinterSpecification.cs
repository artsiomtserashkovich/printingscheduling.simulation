using System.Runtime.Serialization;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    [DataContract]
    public class PrinterSpecification
    {
        public PrinterSpecification(
            int id, 
            double printingSpeed, 
            Dimension printingDimension, 
            double resolution)
        {
            Id = id;
            PrintingSpeed = printingSpeed;
            PrintingDimension = printingDimension;
            Resolution = resolution;
        }
        
        [DataMember(Name = "id")]
        public int Id { get; }
        
        [DataMember(Name = "speed")]
        public double PrintingSpeed { get; }
        
        [DataMember(Name = "dimension")]
        public Dimension PrintingDimension { get; }
        
        [DataMember(Name = "resolution")]
        public double Resolution { get; }
    }
}