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
    internal class ActiveDirectoryUserImportJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                ActiveDirectoryImportUserTask task = new ActiveDirectoryImportUserTask(ConfigurationManager.AppSettings["ActiveDirectoryImportUsersJobDomain"].ToString());
                task.Run();
            });
        }
    }
}
