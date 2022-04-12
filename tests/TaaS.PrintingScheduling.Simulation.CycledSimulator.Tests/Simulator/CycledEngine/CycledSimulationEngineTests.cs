using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Simulator.CycledEngine
{
    public class CycledSimulationEngineTests
    {
        private ICycledManagementActor _managementActorMock;
        private ICycledPrinterActor[] _printerActorsMock;
        private CycledSimulationEngine _sut;

        [SetUp]
        public void SetUp()
        {
            _managementActorMock = Substitute.For<ICycledManagementActor>();
            _printerActorsMock = new []
            {
                Substitute.For<ICycledPrinterActor>(),
                Substitute.For<ICycledPrinterActor>()
            };
            _sut = new CycledSimulationEngine(
                _managementActorMock, 
                _printerActorsMock);
        }

        [Test]
        public void Simulate_ManagementActorNotActive_ShouldNotExecutePrinterActors()
        {
            _managementActorMock.IsComplete.Returns(true);
            
            var results = _sut.Simulate();

            results.Should().BeEmpty();
            _printerActorsMock[0]
                .DidNotReceive()
                .ExecuteCycle();
            _printerActorsMock[1]
                .DidNotReceive()
                .ExecuteCycle();
        }

        [Test]
        public void Simulate_ManagementActorActiveForTwoCycles_ShouldExecutePrinterActorsTwice()
        {
            _managementActorMock.IsComplete.Returns(false, false, true);
            
            var results = _sut.Simulate();

            results.Should().BeEmpty();
            _printerActorsMock[0]
                .Received(Quantity.Exactly(2))
                .ExecuteCycle();
            _printerActorsMock[1]
                .Received(Quantity.Exactly(2))
                .ExecuteCycle();
        }
    }
}