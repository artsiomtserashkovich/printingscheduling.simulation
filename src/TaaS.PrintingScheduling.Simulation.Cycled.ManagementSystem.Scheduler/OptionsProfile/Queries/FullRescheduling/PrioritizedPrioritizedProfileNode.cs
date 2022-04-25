using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling
{
    public abstract class PrioritizedProfileNode<TTime> : 
        PrioritizedOptionsProfile<TTime>, 
        IPrioritizedProfileNode<TTime>
        where TTime : struct
    {
        private readonly Dictionary<OptionIdentifier, IPrioritizedProfileNode<TTime>> _childs;
        
        protected PrioritizedProfileNode(ICollection<PrioritizedScheduleOption<TTime>> options) : base(options)
        {
            _childs = new Dictionary<OptionIdentifier, IPrioritizedProfileNode<TTime>>();
        }

        public IReadOnlyCollection<IPrioritizedProfileNode<TTime>> Childs => _childs.Values;
        
        public abstract IReadOnlyCollection<IPrinterSchedulingState<TTime>> NodeStates { get; }

        public void CreateChild(PrioritizedScheduleOption<TTime> option)
        {
            var optionIdentifier = new OptionIdentifier(option.Job.Specification.Id, option.Printer.Id);

            if (_childs.ContainsKey(optionIdentifier))
            {
                throw new InvalidOperationException("Such child already in node.");
            }
            var childNode = CreateChildNode(option);
            
            childNode.AppendOption(option);
            _childs[optionIdentifier] = childNode;
        }

        protected abstract PrioritizedProfileNode<TTime> CreateChildNode(PrioritizedScheduleOption<TTime> option);
        
        private struct OptionIdentifier
        {
            public int JobId { get; }
        
            public int PrinterId { get; }

            public OptionIdentifier(int jobId, int printerId)
            {
                JobId = jobId;
                PrinterId = printerId;
            }
        }
    }
}
