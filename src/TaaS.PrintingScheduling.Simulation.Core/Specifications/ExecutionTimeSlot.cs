namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    public readonly struct ExecutionTimeSlot<TTime> where TTime : struct
    {
        public TTime Start { get; }
        
        public TTime Finish { get; }

        public ExecutionTimeSlot(TTime start, TTime finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}