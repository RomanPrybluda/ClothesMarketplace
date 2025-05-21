using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Domain
{
    public static class QuartzConfig
    {
        public static void ConfigureQuartz(this IServiceCollection services)
        {

            services.AddSingleton<ImageCleanupService>();

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton(new JobMetadata(typeof(ImageCleanupService), "ImageCleanupJob", "ImageCleanupGroup", "0 0 2 * * ?")); // Start each day in 2:00 pm

            services.AddHostedService<QuartzHostedService>();
        }
    }
}
