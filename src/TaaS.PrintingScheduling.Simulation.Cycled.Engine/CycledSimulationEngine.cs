using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.Engine;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine
{
    public class CycledSimulationEngine : ISimulationEngine
    {
        private readonly ICycledManagementActor _managementActor;
        private readonly IReadOnlyCollection<ICycledPrinterActor> _printerActors;

        public CycledSimulationEngine(
            ICycledManagementActor managementActor,
            IReadOnlyCollection<ICycledPrinterActor> printerActors)
        {
            _managementActor = managementActor;
            _printerActors = printerActors;
        }
        
        public IReadOnlyCollection<JobExecutionResult<long>> Simulate() 
        {
            var executionContext = new CycledSimulationContext();
            
            while (!_managementActor.IsComplete)
            {
                _managementActor.ExecuteCycle(executionContext);
                foreach (var worker in _printerActors)
                {
                    worker.ExecuteCycle();
                }
                
                executionContext.NextCycle();
            }

            return executionContext.GetResults();
        }
    }
}