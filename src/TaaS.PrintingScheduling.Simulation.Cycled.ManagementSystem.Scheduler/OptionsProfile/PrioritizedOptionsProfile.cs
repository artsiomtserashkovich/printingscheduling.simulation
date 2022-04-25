using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile
{
    public abstract class PrioritizedOptionsProfile<TTime> : IPrioritizedOptionsProfile<TTime> where TTime : struct
    {
        private readonly ICollection<PrioritizedScheduleOption<TTime>> _options;

        protected PrioritizedOptionsProfile(ICollection<PrioritizedScheduleOption<TTime>> options)
        {
            _options = options.ToList() ?? throw new ArgumentNullException(nameof(options));
        }

        public double TotalPriority => _options.Sum(option => option.TotalPriority);

        public IReadOnlyCollection<PrioritizedScheduleOption<TTime>> Options => _options.ToArray();

        protected void AppendOption(PrioritizedScheduleOption<TTime> option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            _options.Add(option);
        }
    }    
}
