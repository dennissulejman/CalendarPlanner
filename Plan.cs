using System;
using System.Collections.Generic;

namespace CalendarPlanner
{
    public class Plan
    {
        public string Activity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public Plan()
        {

        }
    }
}