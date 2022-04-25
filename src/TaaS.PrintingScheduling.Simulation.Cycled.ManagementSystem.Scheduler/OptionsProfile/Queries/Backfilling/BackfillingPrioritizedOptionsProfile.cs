using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.Backfilling;

public abstract class BackfillingPrioritizedOptionsProfile<TTime> : PrioritizedOptionsProfile<TTime> where TTime: struct
{
    public abstract IReadOnlyCollection<IPrinterSchedulingState<TTime>> States { get; }

    protected BackfillingPrioritizedOptionsProfile() 
        : base(new List<PrioritizedScheduleOption<TTime>>())
    {
    }

    public abstract void Append(PrioritizedScheduleOption<TTime> option);
}

public class CycledBackfillingOptionsProfile : BackfillingPrioritizedOptionsProfile<long>
{
    private readonly IReadOnlyDictionary<int, CycledSchedulingState> _initialStates;

    public CycledBackfillingOptionsProfile(IReadOnlyCollection<CycledSchedulingState> initialStates)
    {
        _initialStates = initialStates.ToDictionary(state => state.Printer.Id, state => state);
    }

    public override IReadOnlyCollection<IPrinterSchedulingState<long>> States => _initialStates.Values.ToArray();
    
    public override void Append(PrioritizedScheduleOption<long> option)
    {
        _initialStates[option.Printer.Id].AddOption(option);
        AppendOption(option);
    }

    public class CycledSchedulingState : IPrinterSchedulingState<long>
    {
        public PrinterSpecification Printer { get; }

        public long NextAvailableTime { get; private set; }

        public CycledSchedulingState(PrinterSpecification printer, long initialNextAvailableTime)
        {
            Printer = printer;
            NextAvailableTime = initialNextAvailableTime;
        }

        public void AddOption(PrioritizedScheduleOption<long> option)
        {
            var jonDuration = option.TimeSlot.Finish - option.TimeSlot.Start;
            NextAvailableTime += (jonDuration + 1);
        }
    }
}