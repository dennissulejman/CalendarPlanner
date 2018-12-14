using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarPlanner
{
    public class Calendar
    {        
        public Dictionary<DateTime, string> calendar = new Dictionary<DateTime, string>();
        public Dictionary<DateTime, string> GetCalendar()
        {
            return calendar;
        }       
    }
}
