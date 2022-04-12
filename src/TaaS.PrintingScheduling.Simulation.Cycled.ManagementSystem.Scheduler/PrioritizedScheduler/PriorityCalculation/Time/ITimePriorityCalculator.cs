namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Time
{
    public interface ITimePriorityCalculator<in TTime> 
        where TTime : struct
    {
        public double Calculate(TTime scheduled, TTime minimum, TTime maximum);
    }
}