using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ProfilesTree;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration
{
    public class PrioritizedScheduleProfilesQuery<TTime> : IPrioritizedScheduleProfilesQuery<TTime> where TTime : struct
    {
        private readonly IProfilesTreeFactory<TTime> _profilesTreeFactory;
        private readonly IProfileScheduleOptionsQuery<TTime> _optionsQuery;

        public PrioritizedScheduleProfilesQuery(
            IProfilesTreeFactory<TTime> profilesTreeFactory, 
            IProfileScheduleOptionsQuery<TTime> optionsQuery)
        {
            _profilesTreeFactory = profilesTreeFactory;
            _optionsQuery = optionsQuery;
        }

        public IReadOnlyCollection<PrioritizedSchedulesProfile<TTime>> GetProfiles(
            IEnumerable<IPrinterSchedulingState<TTime>> currentStates,
            IEnumerable<PrintingJob<TTime>> jobs,
            TTime schedulingTime)
        {
            var profileTree = _profilesTreeFactory.CreateTree(currentStates);
            var nodes = GenerateProfilesNodes(profileTree, jobs.ToArray(), schedulingTime);
            
            return ExtractProfiles(nodes);
        }

        private IReadOnlyCollection<IProfileNode<TTime>> GenerateProfilesNodes(
            IProfileNode<TTime> rootNode,
            IReadOnlyCollection<PrintingJob<TTime>> jobs, 
            TTime schedulingTime)
        {
            var nodes = new List<IProfileNode<TTime>>();
            var nodesQueue = new Queue<IProfileNode<TTime>>();
            nodesQueue.Enqueue(rootNode);

            while (nodesQueue.Any())
            {
                var iterationProfileNode = nodesQueue.Dequeue();
                var unscheduledJobs = 
                    GetUnscheduledJobs(iterationProfileNode, jobs)
                        .OrderBy(job => job.CommittedFinishTime);

                foreach (var unscheduledJob in unscheduledJobs)
                {
                    var options = _optionsQuery
                        .GetOptions(iterationProfileNode.NodeStates, unscheduledJob, schedulingTime);

                    foreach (var option in options)
                    {
                        iterationProfileNode.AppendOption(option);
                    }
                }

                foreach (var childProfileNode in iterationProfileNode.Childs)
                {
                    nodesQueue.Enqueue(childProfileNode);
                    nodes.Add(childProfileNode);
                }
            }

            return nodes;
        }

        private IReadOnlyCollection<PrintingJob<TTime>> GetUnscheduledJobs(
            IProfileNode<TTime> profileNode,
            IReadOnlyCollection<PrintingJob<TTime>> jobs)
        {
            return jobs
                .Where(job => profileNode.ScheduledJobs
                    .All(option => option.Job.Specification.Id != job.Specification.Id))
                .ToArray();
        }

        private IReadOnlyCollection<PrioritizedSchedulesProfile<TTime>> ExtractProfiles(IEnumerable<IProfileNode<TTime>> nodes)
        {
            return nodes
                .GroupBy(node => node.ScheduledJobs.Count)
                .MaxBy(group => group.Key)?
                .Select(profile => new PrioritizedSchedulesProfile<TTime>(profile.NodeStates, profile.TotalPriority))
                .ToArray();
        }
    }    
}
