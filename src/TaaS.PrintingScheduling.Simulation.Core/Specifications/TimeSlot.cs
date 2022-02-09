namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    public readonly struct TimeSlot<TTime> where TTime : struct
    {
        public TTime Start { get; }
        
        public TTime Finish { get; }

        public TimeSlot(TTime start, TTime finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}