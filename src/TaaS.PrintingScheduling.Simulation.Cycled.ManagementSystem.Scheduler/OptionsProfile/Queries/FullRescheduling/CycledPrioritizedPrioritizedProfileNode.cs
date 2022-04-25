using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling
{
    public class CycledPrioritizedPrioritizedProfileNode : PrioritizedProfileNode<long>
    {
        private readonly IReadOnlyDictionary<int, CycledNodeFlatSchedulingState> _nodeStates;
        
        public CycledPrioritizedPrioritizedProfileNode(IReadOnlyCollection<CycledNodeFlatSchedulingState> initialStates) 
            : base(new List<PrioritizedScheduleOption<long>>())
        {
            _nodeStates = initialStates.ToDictionary(state => state.Printer.Id, state => state);
        }

        private CycledPrioritizedPrioritizedProfileNode(IReadOnlyCollection<CycledNodeFlatSchedulingState> parentStates, ICollection<PrioritizedScheduleOption<long>> parentOptions) 
            : base(parentOptions)
        {
            _nodeStates = parentStates.ToDictionary(state => state.Printer.Id, state => state);
        }

        public override IReadOnlyCollection<IPrinterSchedulingState<long>> NodeStates => _nodeStates.Values.ToArray();

        protected override PrioritizedProfileNode<long> CreateChildNode(PrioritizedScheduleOption<long> option)
        {
            var childStates = _nodeStates
                .Values
                .Select(state => state.Clone())
                .ToArray();
            
            var node = new CycledPrioritizedPrioritizedProfileNode(childStates, Options.ToArray());
            node.ApplyOptionToState(option);

            return node;
        }

        private void ApplyOptionToState(PrioritizedScheduleOption<long> option)
        {
            _nodeStates[option.Printer.Id].AddOption(option);
        }
        
        public class CycledNodeFlatSchedulingState : IPrinterSchedulingState<long>
        {
            public PrinterSpecification Printer { get; }

            public long NextAvailableTime { get; private set; }


            public CycledNodeFlatSchedulingState(PrinterSpecification printer, long initialNextAvailableTime)
            {
                Printer = printer;
                NextAvailableTime = initialNextAvailableTime;
            }

            public CycledNodeFlatSchedulingState Clone()
            {
                return new CycledNodeFlatSchedulingState(
                    Printer, 
                    NextAvailableTime);
            }

            public void AddOption(PrioritizedScheduleOption<long> option)
            {
                var jonDuration = option.TimeSlot.Finish - option.TimeSlot.Start;
                NextAvailableTime += (jonDuration + 1);
            }
        }
    }
}

