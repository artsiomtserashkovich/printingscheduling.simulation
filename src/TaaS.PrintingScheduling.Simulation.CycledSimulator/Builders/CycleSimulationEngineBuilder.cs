using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders
{
    public class CycleSimulationEngineBuilder
    {
        private readonly InMemoryResultsCollector<long> _resultsCollector = new();
        private IReadOnlyCollection<PrinterSpecification> _specifications;
        private CycledPrintingSystem _printingSystem;

        public CycleSimulationEngineBuilder WithPrintingSystem(Action<CycledPrintingSystemBuilder> configure)
        {
            if (_specifications == null)
            {
                throw new InvalidOperationException($"{nameof(_specifications)} is not initialized.");
            }
            if(!_specifications.Any())
            {
                throw new InvalidOperationException($"{nameof(_specifications)} is empty.");
            }
            
            var builder = new CycledPrintingSystemBuilder(_specifications, _resultsCollector);
            configure(builder);
            
            _printingSystem = builder.Build();
            return this;
        }
        
        public CycleSimulationEngineBuilder WithPrinters(IReadOnlyCollection<PrinterSpecification> specifications)
        {
            if (specifications == null)
            {
                throw new InvalidOperationException($"{nameof(specifications)} is not initialized.");
            }
            if(!specifications.Any())
            {
                throw new InvalidOperationException($"{nameof(specifications)} is empty.");
            }

            _specifications = specifications;
            return this;
        }
        
        public ISimulationEngine Build()
        {
            if (_printingSystem == null)
            {
                throw new InvalidOperationException($"{nameof(_printingSystem)} is not initialized.");
            }
            if (_specifications == null)
            {
                throw new InvalidOperationException($"{nameof(_specifications)} is not initialized.");
            }

            var printerActors = _specifications
                .Select(printer => new CycledPrinterActor(printer, _printingSystem))
                .ToArray();

            return new CycledSimulationEngine(
                _printingSystem, 
                printerActors,
                _resultsCollector);
        }
    }
}