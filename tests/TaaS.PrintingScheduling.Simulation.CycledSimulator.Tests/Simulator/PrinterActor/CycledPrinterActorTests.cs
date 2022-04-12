using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Cycled.Jobs;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders;
using TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders.Extensions;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Simulator.PrinterActor
{
    public class CycledPrinterActorTests
    {
        [Test]
        public void Id_IdProvidedInSpecification_ShouldBeSame()
        {
            var specification = new PrinterSpecificationBuilder()
                .WithId(123)
                .Build();

            var sut = new CycledPrinterActor(specification, Substitute.For<IPrintingSystem>());

            sut.Id.Should().Be(123);
        }

        [Test]
        public void ExecuteCycle_NoJobInProgressAndNoNextScheduledJob_ShouldNotRegisterFinishedJob()
        {
            var systemMock = Substitute.For<IPrintingSystem>();
            var specification = new PrinterSpecificationBuilder()
                .WithId(124)
                .Build();
            systemMock.ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 124)).ReturnsNull();
            var sut = new CycledPrinterActor(specification, systemMock);
            
            sut.ExecuteCycle();

            systemMock.DidNotReceive().RegisterFinishedJob(Arg.Is<IPrinter>(p => p.Id == 124));
            systemMock.Received(1).ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 124));
            sut.IsFree.Should().BeTrue();
        }
        
        [Test]
        public void ExecuteCycle_OneJobInSystem_ShouldCompleteJobAndRegisterResult()
        {
            var jobSpecification = new JobSpecificationBuilder<long>()
                .WithDimension(new DimensionBuilder()
                    .WithX(50)
                    .WithY(50)
                    .WithZ(50)
                    .Build())
                .Build();
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithId(125)
                .WithPrintingDimension(dimension => dimension
                    .WithX(100)
                    .WithY(100)
                    .WithZ(100))
                .Build();
            var jobMock = Substitute.For<ICycledJob>();
            var systemMock = Substitute.For<IPrintingSystem>();
            jobMock.Specification.Returns(jobSpecification);
            jobMock.IsComplete.Returns(true);
            systemMock.ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 125)).Returns(jobMock, null);
            var sut = new CycledPrinterActor(printerSpecification, systemMock);
            
            sut.ExecuteCycle();
            sut.ExecuteCycle();

            systemMock.Received(1).RegisterFinishedJob(Arg.Is<IPrinter>(p => p.Id == 125));
            systemMock.Received(2).ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 125));
        }
        
        [Test]
        public void ExecuteCycle_JobSpecificationMoreThanPrinter_ShouldThrowException()
        {
            var jobSpecification = new JobSpecificationBuilder<long>()
                .WithDimension(new DimensionBuilder()
                    .WithX(150)
                    .WithY(50)
                    .WithZ(50)
                    .Build())
                .Build();
            var printerSpecification = new PrinterSpecificationBuilder()
                .WithId(126)
                .WithPrintingDimension(dimesnion => dimesnion
                    .WithX(100)
                    .WithY(100)
                    .WithZ(100))
                .Build();
            var jobMock = Substitute.For<ICycledJob>();
            var systemMock = Substitute.For<IPrintingSystem>();
            jobMock.Specification.Returns(jobSpecification);
            systemMock.ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 126)).Returns(jobMock, null);
            var sut = new CycledPrinterActor(printerSpecification, systemMock);
            
            Action act = () => sut.ExecuteCycle();

            act.Should().Throw<InvalidOperationException>();
            systemMock.DidNotReceive().RegisterFinishedJob(Arg.Is<IPrinter>(p => p.Id == 126));
            systemMock.Received(1).ScheduleNextJob(Arg.Is<IPrinter>(p => p.Id == 126));
            jobMock.DidNotReceive().Execute(printerSpecification);
            sut.IsFree.Should().BeTrue();
        }
    }
}