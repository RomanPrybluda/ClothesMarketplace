using Microsoft.Extensions.Hosting;
using Quartz;

namespace Domain
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEnumerable<JobMetadata> _jobMetadata;
        private IScheduler _scheduler;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IEnumerable<JobMetadata> jobMetadata)
        {
            _schedulerFactory = schedulerFactory;
            _jobMetadata = jobMetadata;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler();

            foreach (var jobMeta in _jobMetadata)
            {
                var jobDetail = JobBuilder.Create(jobMeta.JobType)
                    .WithIdentity(jobMeta.JobName, jobMeta.JobGroup)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobMeta.JobName}_trigger", jobMeta.JobGroup)
                    .WithCronSchedule(jobMeta.CronExpression)
                    .Build();

                await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_scheduler != null)
            {
                await _scheduler.Shutdown(cancellationToken);
            }
        }
    }
}
