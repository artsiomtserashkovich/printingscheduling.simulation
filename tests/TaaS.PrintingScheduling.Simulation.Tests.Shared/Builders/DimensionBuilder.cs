using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders
{
    public class DimensionBuilder
    {
        private double _x;
        private double _y;
        private double _z;
        
        public Dimension Build()
        {
            return new Dimension(_x, _y, _z);
        }

        public DimensionBuilder WithX(double x)
        {
            _x = x;
            return this;
        }
        
        public DimensionBuilder WithY(double y)
        {
            _y = y;
            return this;
        }
        
        public DimensionBuilder WithZ(double z)
        {
            _z = z;
            return this;
        }
    }
}