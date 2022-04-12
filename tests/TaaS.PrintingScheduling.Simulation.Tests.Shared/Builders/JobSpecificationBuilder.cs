using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Tests.Shared.Builders
{
    public class JobSpecificationBuilder<TTime> where TTime : struct
    {
        private int _id;
        private double _resolution;
        private Dimension _dimension;
        private TTime _incomingTime;
        private double _priorityCoefficient;
        
        public JobSpecification<TTime> Build()
        {
            return new JobSpecification<TTime>(_id, _resolution, _dimension, _incomingTime, _priorityCoefficient);
        }

        public JobSpecificationBuilder<TTime> WithId(int id)
        {
            _id = id;
            return this;
        }
        
        public JobSpecificationBuilder<TTime> WithResolution(double resolution)
        {
            _resolution = resolution;
            return this;
        }
        
        public JobSpecificationBuilder<TTime> WithDimension(Dimension dimension)
        {
            _dimension = dimension;
            return this;
        }
        
        public JobSpecificationBuilder<TTime> WithIncomingTime(TTime incomingTime)
        {
            _incomingTime = incomingTime;
            return this;
        }
        
        public JobSpecificationBuilder<TTime> WithPriorityCoefficient(double priorityCoefficient)
        {
            _priorityCoefficient = priorityCoefficient;
            return this;
        }
    }
}