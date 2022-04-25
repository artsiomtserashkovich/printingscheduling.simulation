using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.Backfilling
{
    public class BackfillingProfilesQuery<TTime> : IPrioritizedScheduleProfilesQuery<TTime> where TTime :struct
    {
        private readonly IGenerateJobsSequencesCommand<TTime> _generateSequencesCommand;
        private readonly IBackfillingProfileFactory<TTime> _profileFactory;
        private readonly IPrioritizedScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly IPrioritizedOptionChooser<TTime> _optionChooser;

        public BackfillingProfilesQuery(
            IGenerateJobsSequencesCommand<TTime> generateSequencesCommand,
            IBackfillingProfileFactory<TTime> profileFactory,
            IPrioritizedScheduleOptionsQuery<TTime> optionsQuery, 
            IPrioritizedOptionChooser<TTime> optionChooser)
        {
            _generateSequencesCommand = generateSequencesCommand;
            _profileFactory = profileFactory;
            _optionsQuery = optionsQuery;
            _optionChooser = optionChooser;
        }

        public IReadOnlyCollection<IPrioritizedOptionsProfile<TTime>> GetProfiles(
            IEnumerable<IPrinterSchedulingState<TTime>> baseStates, 
            IEnumerable<PrintingJob<TTime>> previousScheduleJobs,
            PrintingJob<TTime> job,
            TTime schedulingTime)
        {
            var sequences = _generateSequencesCommand.Generate(previousScheduleJobs, job);

            return sequences
                .Select(sequence => GetProfile(baseStates, sequence, schedulingTime))
                .Where(profile => profile != null)
                .ToArray()!;
        }

        private IPrioritizedOptionsProfile<TTime>? GetProfile(
            IEnumerable<IPrinterSchedulingState<TTime>> baseStates,
            Queue<PrintingJob<TTime>> jobs, 
            TTime schedulingTime)
        {
            var profile = _profileFactory.CreateFromStates(baseStates);

            while (jobs.Any())
            {
                var job = jobs.Dequeue();

                var options = _optionsQuery.GetOptions(profile.States, job, schedulingTime);
                var option = _optionChooser.ChooseOption(options);
                if (option == null)
                {
                    return null;
                }
                
                profile.Append(option);
            }

            return profile;
        }
    }
}