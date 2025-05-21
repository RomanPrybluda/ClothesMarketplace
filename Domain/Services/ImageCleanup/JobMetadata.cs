namespace Domain
{
    public class JobMetadata
    {
        public Type JobType { get; }
        public string JobName { get; }
        public string JobGroup { get; }
        public string CronExpression { get; }

        public JobMetadata(Type jobType, string jobName, string jobGroup, string cronExpression)
        {
            JobType = jobType;
            JobName = jobName;
            JobGroup = jobGroup;
            CronExpression = cronExpression;
        }
    }
}
