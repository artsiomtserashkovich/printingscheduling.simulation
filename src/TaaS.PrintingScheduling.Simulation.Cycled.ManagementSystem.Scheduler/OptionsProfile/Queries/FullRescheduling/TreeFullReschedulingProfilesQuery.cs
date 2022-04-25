using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node.Factory;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling
{
    public class TreeFullReschedulingProfilesQuery<TTime> : IPrioritizedScheduleProfilesQuery<TTime> where TTime : struct
    {
        private readonly IPrioritizedProfileNodeFactory<TTime> _profileNodeFactory;
        private readonly IPrioritizedScheduleOptionsQuery<TTime> _optionsQuery;

        public TreeFullReschedulingProfilesQuery(
            IPrioritizedProfileNodeFactory<TTime> profileNodeFactory, 
            IPrioritizedScheduleOptionsQuery<TTime> optionsQuery)
        {
            _profileNodeFactory = profileNodeFactory;
            _optionsQuery = optionsQuery;
        }

        public IReadOnlyCollection<IPrioritizedOptionsProfile<TTime>> GetProfiles(
            IEnumerable<IPrinterSchedulingState<TTime>> baseStates,
            IEnumerable<PrintingJob<TTime>> previousScheduledJobs,
            PrintingJob<TTime> job,
            TTime schedulingTime)
        {
            var jobs = previousScheduledJobs.Concat(new[] { job });
            
            var profileTree = _profileNodeFactory.CreateRootNode(baseStates);
            var nodes = GenerateProfilesNodes(profileTree, jobs.ToArray(), schedulingTime);
            
            return ExtractProfiles(nodes);
        }

        private IReadOnlyCollection<IPrioritizedProfileNode<TTime>> GenerateProfilesNodes(
            IPrioritizedProfileNode<TTime> rootNode,
            IReadOnlyCollection<PrintingJob<TTime>> jobs, 
            TTime schedulingTime)
        {
            var nodes = new List<IPrioritizedProfileNode<TTime>>();
            var nodesQueue = new Queue<IPrioritizedProfileNode<TTime>>();
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
                        iterationProfileNode.CreateChild(option);
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
            IPrioritizedProfileNode<TTime> prioritizedProfileNode,
            IReadOnlyCollection<PrintingJob<TTime>> jobs)
        {
            return jobs
                .Where(job => prioritizedProfileNode.Options
                    .All(option => option.Job.Specification.Id != job.Specification.Id))
                .ToArray();
        }

        private IReadOnlyCollection<IPrioritizedOptionsProfile<TTime>> ExtractProfiles(IEnumerable<IPrioritizedOptionsProfile<TTime>> nodes)
        {
            return nodes
                .GroupBy(node => node.Options.Count)
                .MaxBy(group => group.Key)?
                .ToArray();
        }
    }    
}
