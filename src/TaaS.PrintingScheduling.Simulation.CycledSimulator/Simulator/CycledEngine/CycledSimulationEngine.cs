using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine
{
    public class CycledSimulationEngine : ISimulationEngine
    {
        private readonly ICycledManagementActor _managementActor;
        private readonly IReadOnlyCollection<ICycledPrinterActor> _printingExecutors;
        
        public CycledSimulationEngine(
            ICycledManagementActor managementActor,
            IReadOnlyCollection<ICycledPrinterActor> printingExecutors)
        {
            _managementActor = managementActor;
            _printingExecutors = printingExecutors;
        }
        
        public void Simulate() 
        {
            var executionContext = new CycledSimulationContext();
            
            while (!_managementActor.IsComplete)
            {
                _managementActor.ExecuteManagingCycle(executionContext);
                foreach (var worker in _printingExecutors)
                {
                    worker.ExecuteCycle(executionContext);
                }
                
                executionContext.NextCycle();
            }
        }

    }
}