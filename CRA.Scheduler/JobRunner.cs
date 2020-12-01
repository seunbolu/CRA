using CRA.Scheduler.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Scheduler
{
    public partial class JobRunner : ServiceBase
    {
        private List<IScheduler> _schedulers;

        public JobRunner()
        {
            InitializeComponent();
        }

        private void CreateSchedulers()
        {
            NameValueCollection props = new NameValueCollection {
                { "quartz.serializer.type", "binary" },
                { "quartz.threadPool.threadCount", "20" }
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            ScheduleActiveDirectoryImportJob(factory);
            ScheduleProcessEmailQueueJob(factory);
        }

        private void ScheduleActiveDirectoryImportJob(StdSchedulerFactory factory)
        {
            IScheduler sched = factory.GetScheduler().Result;
            _schedulers.Add(sched);
            sched.Start().Wait();
            IJobDetail job = JobBuilder.Create<ActiveDirectoryUserImportJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
               
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(int.Parse(ConfigurationManager.AppSettings["ActiveDirectoryImportUsersJobInterval"].ToString())).RepeatForever())
                .Build();

            sched.ScheduleJob(job, trigger).Wait();
        }

        private void ScheduleProcessEmailQueueJob(StdSchedulerFactory factory)
        {
            IScheduler sched = factory.GetScheduler().Result;
            _schedulers.Add(sched);
            sched.Start().Wait();
            IJobDetail job = JobBuilder.Create<ProcessQueuedEmailJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(int.Parse(ConfigurationManager.AppSettings["ProcessEmailQueueJobInterval"].ToString()))
                .WithMisfireHandlingInstructionIgnoreMisfires()
                .RepeatForever())
                .Build();

            sched.ScheduleJob(job, trigger).Wait();
        }


        protected override void OnStart(string[] args)
        {
            _schedulers = new List<IScheduler>();
            CreateSchedulers();
        }

        protected override void OnStop()
        {
            if (_schedulers != null)
            {
                foreach (var item in _schedulers)
                {
                    item.Shutdown(false).Wait();
                }
            }
        }
    }
}
