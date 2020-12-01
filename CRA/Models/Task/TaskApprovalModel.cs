using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Task
{
    public class TaskApprovalModel
    {
        public long UserTaskId { get; set; }
        public string Action { get; set; }

        public string Notes { get; set; }
    }
}