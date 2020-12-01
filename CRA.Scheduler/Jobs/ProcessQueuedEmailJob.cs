using CRA.Tasks.Model;
using CRA.Tasks.Tasks;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Scheduler.Jobs
{
    internal class ProcessQueuedEmailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {

                SmtpOptions options = new SmtpOptions();

                //options.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                options.FromAddress = ConfigurationManager.AppSettings["SmtpFromAddress"];
                //options.Password = ConfigurationManager.AppSettings["SmtpPassword"];
                //options.FromName = ConfigurationManager.AppSettings["SmtpFromName"];
                options.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                options.HostName = ConfigurationManager.AppSettings["SmtpHostName"];

                ProcessQueuedEmailTask task = new ProcessQueuedEmailTask(int.Parse(ConfigurationManager.AppSettings["EmailBatchSize"]), options);

                task.Run();
            });
        }
    }
}
