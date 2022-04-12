using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders
{
    public class PrinterSpecificationBuilder
    {
        private int _id;
        private double _printingSpeed;
        private Dimension _printingDimension;
        private double _resolution;
        
        public PrinterSpecification Build()
        {
            return new PrinterSpecification(_id, _printingSpeed, _printingDimension, _resolution);
        }

        public PrinterSpecificationBuilder WithId(int id)
        {
            _id = id;
            return this;
        }
        
        public PrinterSpecificationBuilder WithPrintingSpeed(double printingSpeed)
        {
            _printingSpeed = printingSpeed;
            return this;
        }
        
        public PrinterSpecificationBuilder WithPrintingDimension(Dimension printingDimension)
        {
            _printingDimension = printingDimension;
            return this;
        }
        
        public PrinterSpecificationBuilder WithResolution(double resolution)
        {
            _resolution = resolution;
            return this;
        }
    }
}