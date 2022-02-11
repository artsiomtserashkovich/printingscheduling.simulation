using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        
        [JsonPropertyName("id")]
        public int Id { get; }
        
        [JsonPropertyName("speed")]
        public double PrintingSpeed { get; }
        
        public Dimension PrintingDimension { get; }
        
        [JsonPropertyName("resolution")]
        public double Resolution { get; }
    }
}