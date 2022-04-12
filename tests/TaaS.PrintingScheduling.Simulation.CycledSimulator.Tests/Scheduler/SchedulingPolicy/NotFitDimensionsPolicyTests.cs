using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders.Extensions;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Scheduler.SchedulingPolicy
{
    public class NotFitDimensionsPolicyTests
    {
        private readonly ISchedulingPolicy<DateTime> _innerPolicyMock;
        private NotFitDimensionsPolicy<DateTime> _sut;

        public NotFitDimensionsPolicyTests()
        {
            _innerPolicyMock = Substitute.For<ISchedulingPolicy<DateTime>>();
            _sut = new NotFitDimensionsPolicy<DateTime>(_innerPolicyMock);
        }

        [Test]
        public void IsAllowed_JobFitAndInnerSucceed_ShouldBeTrue()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(true);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithPrintingDimension(dimension => dimension
                    .WithX(200)
                    .WithY(200)
                    .WithZ(200))
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithDimension(new DimensionBuilder()
                    .WithX(200)
                    .WithY(200)
                    .WithZ(200)
                    .Build())
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeTrue();
        }
        
        [Test]
        public void IsAllowed_JobFitAndInnerNotSucceed_ShouldBeFalse()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(false);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithPrintingDimension(dimension => dimension
                    .WithX(200)
                    .WithY(200)
                    .WithZ(200))
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithDimension(new DimensionBuilder()
                    .WithX(150)
                    .WithY(150)
                    .WithZ(150)
                    .Build())
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeFalse();
        }
        
        [Test]
        public void IsAllowed_JobNotFit_ShouldBeFalse()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(true);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithPrintingDimension(dimension => dimension
                    .WithX(200)
                    .WithY(200)
                    .WithZ(200))
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithDimension(new DimensionBuilder()
                    .WithX(200)
                    .WithY(250)
                    .WithZ(200)
                    .Build())
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeFalse();
        }
    }
}