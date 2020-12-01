using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.CallCycle
{
    public class CallCycleModel
    {

        public List<DayHeaderModel> DayHeaders { get; set; }
        public List<TimeHeaderModel> TimeHeaders { get; set; }

        void PopulateDayHeaders()
        {
            DayHeaders.Add(new DayHeaderModel() { Name = "Sunday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Monday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Tuesday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Wednesday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Thursday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Friday" });
            DayHeaders.Add(new DayHeaderModel() { Name = "Saturday" });
        }


        void PopulateTimeHeaders()
        {
            TimeHeaders.Add(new TimeHeaderModel() { Name = "AM" });
            TimeHeaders.Add(new TimeHeaderModel() { Name = "PM" });

        }

        public CallCycleModel()
        {
            DayHeaders = new List<DayHeaderModel>();
            TimeHeaders = new List<TimeHeaderModel>();
            PopulateDayHeaders();
            PopulateTimeHeaders();

        }
    }
}