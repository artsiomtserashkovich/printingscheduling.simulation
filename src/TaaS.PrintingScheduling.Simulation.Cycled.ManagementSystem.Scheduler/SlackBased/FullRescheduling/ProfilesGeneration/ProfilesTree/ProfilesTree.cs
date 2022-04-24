using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ProfilesTree
{
    public interface IProfileNode<TTime> where TTime : struct
    {
        public double TotalPriority { get; }

        public IReadOnlyCollection<IProfileNode<TTime>> Childs { get; }
        
        public IReadOnlyCollection<IPrinterProfileSchedulingState<TTime>> NodeStates { get; }
        
        public IReadOnlyCollection<ProfileScheduleOption<TTime>> ScheduledJobs { get; }

        public void AppendOption(ProfileScheduleOption<TTime> option);
    }

    public class CycledProfileNode : IProfileNode<long>
    {
        private readonly Dictionary<OptionIdentifier, CycledProfileNode> _childs;
        private readonly IReadOnlyCollection<CycledNodeSchedulingState> _nodeStates;
            
        public CycledProfileNode(IReadOnlyCollection<CycledNodeSchedulingState> nodeStates)
        {
            TotalPriority = 0;
            _nodeStates = nodeStates;
            _childs = new Dictionary<OptionIdentifier, CycledProfileNode>();
        }

        private CycledProfileNode(double totalPriority, IReadOnlyCollection<CycledNodeSchedulingState> nodeStates)
        {
            TotalPriority = totalPriority;
            _nodeStates = nodeStates;
            _childs = new Dictionary<OptionIdentifier, CycledProfileNode>();
        }

        public double TotalPriority { get; }

        public IReadOnlyCollection<ProfileScheduleOption<long>> ScheduledJobs =>
            _nodeStates
                .SelectMany(state => state.Schedules)
                .ToArray();

        public IReadOnlyCollection<IPrinterProfileSchedulingState<long>> NodeStates => _nodeStates.ToArray();

        public IReadOnlyCollection<IProfileNode<long>> Childs => _childs.Values;

        public void AppendOption(ProfileScheduleOption<long> option)
        {
            var optionIdentifier = new OptionIdentifier(option.Job.Specification.Id, option.Printer.Id);

            if (_childs.ContainsKey(optionIdentifier))
            {
                throw new InvalidOperationException("Such child already in profile node.");
            }

            var childStates = _nodeStates
                .Select(state => state.Clone())
                .ToArray();
            
            childStates
                .First(state => state.Printer.Id == option.Printer.Id)
                .AddOption(option);

            _childs[optionIdentifier] = new CycledProfileNode(TotalPriority + option.TotalPriority, childStates);
        }
    }

    public class CycledNodeSchedulingState : IPrinterProfileSchedulingState<long>
    {
        private readonly Queue<ProfileScheduleOption<long>> _queue;
        
        public PrinterSpecification Printer { get; }

        public long NextAvailableTime { get; private set; }

        public IReadOnlyCollection<ProfileScheduleOption<long>> Schedules => _queue;

        public CycledNodeSchedulingState(PrinterSpecification printer, long initialNextAvailableTime)
        {
            Printer = printer;
            NextAvailableTime = initialNextAvailableTime;
            _queue = new Queue<ProfileScheduleOption<long>>();
        }

        private CycledNodeSchedulingState(
            PrinterSpecification printer, 
            long initialNextAvailableTime,
            Queue<ProfileScheduleOption<long>> queue)
        {
            Printer = printer;
            NextAvailableTime = initialNextAvailableTime;
            _queue = queue;
        }

        public CycledNodeSchedulingState Clone()
        {
            return new CycledNodeSchedulingState(
                Printer, 
                NextAvailableTime, 
                new Queue<ProfileScheduleOption<long>>(_queue));
        }

        public void AddOption(ProfileScheduleOption<long> option)
        {
            _queue.Enqueue(option);
            var jonDuration = option.TimeSlot.Finish - option.TimeSlot.Start;
            NextAvailableTime += (jonDuration + 1);
        }
    }

    public struct OptionIdentifier
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

