using System;

namespace TaaS.PrintingScheduling.Simulation.Core.Specifications
{
    public class PrintingJob<TTime> where TTime : struct
    {
        public JobSpecification<TTime> Specification { get; }

        public TTime CommittedFinishTime { get; }

        public PrintingJob(JobSpecification<TTime> specification, TTime finishTimeDeadline)
        {
            Specification = specification;
            CommittedFinishTime = finishTimeDeadline;
        }
    }
}