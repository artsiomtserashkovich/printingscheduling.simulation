namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Resolution
{
    public interface IResolutionPriorityCalculator
    {
        public double Calculate(ResolutionPriorityParameters parameters);
    }
}