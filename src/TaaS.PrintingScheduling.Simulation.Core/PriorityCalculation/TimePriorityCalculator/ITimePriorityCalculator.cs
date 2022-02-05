using System;

namespace TaaS.PrintingScheduling.Simulation.Core.PriorityCalculation.TimePriorityCalculator
{
    public interface ITimePriorityCalculator<in TTime> 
        where TTime : struct
    {
        public double Calculate(TTime cycle);
    }
}