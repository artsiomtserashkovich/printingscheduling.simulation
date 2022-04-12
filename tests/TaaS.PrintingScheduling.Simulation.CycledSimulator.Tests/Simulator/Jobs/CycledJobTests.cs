using FluentAssertions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Cycled.Jobs;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Simulator.Jobs
{
    public class CycledJobTests
    {
        [Test]
        public void Specification_ProvideJobSpecification_ShouldBeTheSame()
        {
            var specification = new JobSpecificationBuilder<long>()
                .WithId(123)
                .WithDimension(new DimensionBuilder()
                    .WithX(30)
                    .WithY(40)
                    .WithZ(50)
                    .Build())
                .Build();
            var sut = new CycledJob(specification);

            sut.Specification.Should().Be(specification);
        }
        
        [Test]
        public void Execute_PrintingVolumeFor2Cycles_ShouldBeCompleted()
        {
            var specification = new JobSpecificationBuilder<long>()
                .WithId(123)
                .WithDimension(new DimensionBuilder()
                    .WithX(4)
                    .WithY(4)
                    .WithZ(4)
                    .Build())
                .Build();
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithPrintingSpeed(20)
                .WithResolution(0.4)
                .Build();
            var sut = new CycledJob(specification);

            sut.Execute(printerSpecification);
            sut.IsComplete.Should().BeFalse();
            
            sut.Execute(printerSpecification);
            sut.IsComplete.Should().BeTrue();
        }
    }
}