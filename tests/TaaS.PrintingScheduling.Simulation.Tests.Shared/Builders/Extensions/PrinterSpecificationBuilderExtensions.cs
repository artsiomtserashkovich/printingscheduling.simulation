using System;

namespace TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders.Extensions
{
    public static class PrinterSpecificationBuilderExtensions
    {
        public static PrinterSpecificationBuilder WithPrintingDimension(
            this PrinterSpecificationBuilder builder, 
            Action<DimensionBuilder> configure)
        {
            var dimensionBuilder = new DimensionBuilder();
            configure(dimensionBuilder);
            
            return builder.WithPrintingDimension(dimensionBuilder.Build());
        }
    }
}