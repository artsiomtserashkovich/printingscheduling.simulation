using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.Backfilling
{
    public interface IGenerateJobsSequencesCommand<TTime> where TTime : struct
    {
        IReadOnlyCollection<Queue<PrintingJob<TTime>>> Generate(
            IEnumerable<PrintingJob<TTime>> baseSequences,
            PrintingJob<TTime> job);
    }    
    
    public class GenerateSequencesCommand<TTime> : IGenerateJobsSequencesCommand<TTime> 
        where TTime : struct
    {
        public IReadOnlyCollection<Queue<PrintingJob<TTime>>> Generate(IEnumerable<PrintingJob<TTime>> baseSequences, PrintingJob<TTime> job)
        {
            var sequences = new Stack<Queue<PrintingJob<TTime>>>();
            
            baseSequences = baseSequences
                .OrderBy(j => j.Specification.IncomingTime)
                .ToArray();

            for (int index = 0; index < baseSequences.Count() + 1; index++)
            {
                var sequence = new Queue<PrintingJob<TTime>>();

                foreach (var j in baseSequences.Take(index))
                {
                    sequence.Enqueue(j);
                }
                
                sequence.Enqueue(job);
                
                foreach (var j in baseSequences.Skip(index))
                {
                    sequence.Enqueue(j);
                }
                
                sequences.Push(sequence);
            }
            
            return sequences.ToArray();
        }
    }
}
