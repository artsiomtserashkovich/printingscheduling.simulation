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
        
        public int Id { get; }
        
        public double PrintingSpeed { get; }
        
        public Dimension PrintingDimension { get; }
        
        public double Resolution { get; }
    }
}