namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.TimePriorityCalculator
{
    public interface ITimePriorityCalculator<in TTime> 
        where TTime : struct
    {
        public double Calculate(TTime cycle);
    }
}