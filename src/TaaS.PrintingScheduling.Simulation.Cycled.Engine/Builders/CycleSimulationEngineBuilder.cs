using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Engine.Builders
{
    public class CycleSimulationEngineBuilder
    {
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
            
            var builder = new CycledPrintingSystemBuilder(_specifications);
            configure(builder);
            
            _printingSystem = builder.Build();
            return this;
        }
        
        public CycleSimulationEngineBuilder WithPrinters(IReadOnlyCollection<PrinterSpecification> specifications)
        {
            if (specifications == null || !specifications.Any())
            {
                throw new InvalidOperationException($"{nameof(specifications)} is null or empty.");
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
                printerActors);
        }
    }
}