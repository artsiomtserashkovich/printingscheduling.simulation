using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Scheduler.SchedulingPolicy
{
    public class WorseResolutionPolicyTests
    {
        private readonly ISchedulingPolicy<DateTime> _innerPolicyMock;
        private readonly WorseResolutionPolicy<DateTime> _sut;

        public WorseResolutionPolicyTests()
        {
            _innerPolicyMock = Substitute.For<ISchedulingPolicy<DateTime>>();
            _sut = new WorseResolutionPolicy<DateTime>(_innerPolicyMock);
        }

        [Test]
        public void IsAllowed_PrinterResolutionBetterThanJobAndInnerSucceed_ShouldBeTrue()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(true);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithResolution(0.6)
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithResolution(0.7)
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeTrue();
        }
        
        [Test]
        public void IsAllowed_PrinterResolutionBetterThanJobAndInnerNotSucceed_ShouldBeFalse()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(false);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithResolution(0.6)
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithResolution(0.7)
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeFalse();
        }
        
        [Test]
        public void IsAllowed_PrinterResolutionWorseThanJob_ShouldBeFalse()
        {
            _innerPolicyMock
                .IsAllowed(Arg.Any<PrinterSpecification>(), Arg.Any<JobSpecification<DateTime>>())
                .Returns(true);
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithResolution(0.4)
                .Build();
            var jobSpecification = new JobSpecificationBuilder<DateTime>()
                .WithResolution(0.2)
                .Build();
            
            var result = _sut.IsAllowed(printerSpecification, jobSpecification);

            result.Should().BeFalse();
        }
    }
}