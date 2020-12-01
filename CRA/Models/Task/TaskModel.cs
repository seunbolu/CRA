using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Task
{
    public class TaskModel
    {
        public long UserTaskId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string TaskType { get; set; }
        public string Notes { get; set; }
        public string UserNotes { get; set; }

        public string Created { get; set; }
        public string Modified { get; set; }


        public long ScheduleId { get; set; }

        public long ReferralSourceId{ get; set; }

        public long ContactId { get; set; }
        public long PreScreenId { get; set; }




    }
}