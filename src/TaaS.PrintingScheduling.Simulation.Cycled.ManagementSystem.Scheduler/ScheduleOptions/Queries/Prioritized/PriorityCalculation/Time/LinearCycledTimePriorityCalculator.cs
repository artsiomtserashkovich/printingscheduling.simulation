namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Time
{
    public class LinearTimePriorityCalculator : ITimePriorityCalculator<long>
    {
        private const int FloatPartPrecision = 3;

        public double Calculate(long scheduled, long minimum, long maximum)
        {
            AssertCalculateParameters(scheduled, minimum, maximum);

            if (scheduled == minimum && minimum == maximum)
            {
                return 1;
            }

            var result = 1 - (double) (scheduled - minimum) / (maximum - minimum);
            
            return Math.Round(result, FloatPartPrecision);
        }
        
        
        private void AssertCalculateParameters(long scheduled, long minimum, long maximum)
        {
            if (minimum <= 0)
            {
                throw new ArgumentException("Can't be 0 or negative.", nameof(minimum));
            }
            
            if (maximum <= 0)
            {
                throw new ArgumentException("Can't be 0 or negative.", nameof(maximum));
            }

            if (minimum > maximum)
            {
                throw new ArgumentException(
                    $"{nameof(minimum)} can't be equal {nameof(maximum)} with value: '{maximum}'." +
                    $" Current value: '{minimum}'.",
                    nameof(minimum));
            }

            if (scheduled < minimum)
            {
                throw new ArgumentException(
                    $"Can't calculate priority for value that less than {nameof(minimum)} " +
                    $"with value: '{minimum}'.");
            }

            if (scheduled > maximum)
            {
                throw new ArgumentException(
                    $"Can't calculate priority for value that more than {nameof(maximum)} "+
                    $"with value: '{maximum}'.");
            }
        }
    }
}