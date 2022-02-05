using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.PrintingResult;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrintingSystem.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator
{
    public interface ISimulationEngine
    {
        void Simulate();
    }
}