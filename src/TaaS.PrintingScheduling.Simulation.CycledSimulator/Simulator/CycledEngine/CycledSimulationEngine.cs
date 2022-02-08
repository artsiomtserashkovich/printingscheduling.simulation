using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

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
        
        public void Simulate() 
        {
            var executionContext = new CycledSimulationContext();
            
            while (!_managementActor.IsComplete)
            {
                _managementActor.ExecuteManagingCycle(executionContext);
                foreach (var worker in _printerActors)
                {
                    worker.ExecuteCycle(executionContext);
                }
                
                executionContext.NextCycle();
            }
        }

    }
}