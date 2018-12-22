using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace CalendarPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Calendar calendar = new Calendar();

                Console.Clear();
                Console.WriteLine("Welcome! What would you like to do?");
                Console.WriteLine("Type the corresponding number then press enter to execute:");
                Console.WriteLine("1: Create a new calendar-planner.");
                Console.WriteLine("2: Add Plans.");
                Console.WriteLine("3: Remove Plans.");
                Console.WriteLine("4: Display your existing calendar-planner.");
                Console.WriteLine();
                Console.WriteLine("5: Close the application.");
                string response = Console.ReadLine();
                if (response == "1")
                {
                    CreateCalendarPlanner(ref calendar);
                    AddPlan(ref calendar);
                }
                else if (response == "2")
                {
                    AddPlan(ref calendar);                    
                }
                else if (response == "3")
                {
                    RemovePlan(ref calendar);                    
                }
                else if (response == "4")
                {
                    DisplayCalendarPlanner();                    
                }
                else if (response == "5")
                {
                    Environment.Exit(0);
                }
                Console.ReadLine();
            }
        }

        public static void CreateCalendarPlanner(ref Calendar calendar)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            
            FileStream createNewPlanner;
            bool detectCalendarPlanner = true;
            while (detectCalendarPlanner)
            {
                if (File.Exists(path + @"\Calendar.csv"))
                {
                    Console.WriteLine("Found existing Calendar-Planner.");
                    Console.WriteLine("Would you like to replace your current Planner?(y/n)");
                    string response = Console.ReadLine();
                    if (response == "y")
                    {
                        createNewPlanner = File.Create(path + @"\Calendar.csv");
                        createNewPlanner.Close();
                        detectCalendarPlanner = false;
                    }
                    else if (response == "n")
                    {
                        detectCalendarPlanner = false;
                    }
                    else
                    {
                        detectCalendarPlanner = true;
                        Console.Clear();
                        Console.WriteLine("Invalid input format, try again.");
                        Console.WriteLine();
                    }
                }
                else
                {
                    createNewPlanner = File.Create(path + @"\Calendar.csv");
                    createNewPlanner.Close();
                    detectCalendarPlanner = false;
                }
            }
        }

        public static void GetCalendarPlanner(ref Calendar calendar)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            StreamReader calendarReader = new StreamReader(path + @"\Calendar.csv");
            using (calendarReader)

            {
                calendarReader.ReadLine();
                calendarReader.ReadLine();
                while (calendarReader.Peek() != -1)
                {
                    string line = calendarReader.ReadLine();
                    string[] columns = line.Split('-');

                    calendar.AddPlanEntry(columns[0], columns[1], columns[2]);
                }
            }
            calendarReader.Close();

        }

        public static void DisplayCalendarPlanner()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string read = File.ReadAllText(path + @"\Calendar.csv");
            Console.WriteLine(read);
        }

        public static void SaveCalendarPlanner(ref Calendar calendar)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.WriteLine("Here are your new plans for the year:");
            Console.WriteLine();
            StringBuilder Plans = new StringBuilder();
            Plans.AppendLine("My Calendar:");
            Plans.AppendLine();
            foreach (Plan plan in calendar.Plans.OrderBy(v => v.StartTime))
            {
                Console.WriteLine(plan.Display, Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(plan.Activity));
                string dsa = (plan.Display);
                Plans.AppendLine(plan.Display);
                File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());
            }
        }

        public static void AddPlan(ref Calendar calendar)
        {
            GetCalendarPlanner(ref calendar);
            bool add = true;
            while (add)
            {
                Console.Clear();
                Console.WriteLine("What are you planning to do?");
                string activity = Console.ReadLine();
                Console.WriteLine("Which date and time does it start?(dd/mm/yy hh:mm)");
                string startTime = Console.ReadLine();
                ValidateAddDateFormat(ref calendar, startTime);
                Console.WriteLine("Until which date and time does it last?(dd/mm/yy hh:mm)");
                string endTime = Console.ReadLine();
                ValidateAddDateFormat(ref calendar, endTime);
                calendar.AddPlanEntry(startTime, endTime, activity);
                Console.WriteLine("Are you done adding plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    add = true;
                    Console.Clear();
                }
                else if (response == "y")
                {
                    add = false;
                }
                SaveCalendarPlanner(ref calendar);
            }
        }

        public static void RemovePlan(ref Calendar calendar)
        {
            GetCalendarPlanner(ref calendar);
            bool remove = true;
            while (remove)
            {
                Console.Clear();
                Console.WriteLine("When is the plan you would like to remove?");
                string removeDate = Console.ReadLine();               
                Console.WriteLine("Which plan would you like to remove?");
                string activity = Console.ReadLine().ToUpper();
                ValidateRemoveDateFormat(ref calendar, removeDate, activity.ToUpper());
                calendar.RemovePlanEntry(removeDate, activity.ToUpper());
                Console.WriteLine("Are you done removing plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    remove = true;
                    Console.Clear();
                }
                else if (response == "y")
                {
                    remove = false;
                }
                SaveCalendarPlanner(ref calendar);
            }
        }

        public static void ValidateAddDateFormat(ref Calendar calendar, string date)
        {
            bool addDateFormat = true;
            addDateFormat &= DateTime.TryParse(date, out DateTime result);

            if (!addDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                AddPlan(ref calendar);
            }          
        }

        public static void ValidateRemoveDateFormat(ref Calendar calendar, string date, string activity)
        {
            bool removeDateFormat = true;
            bool planActivity = true;
            removeDateFormat &= DateTime.TryParse(date, out DateTime result);
            planActivity &= calendar.Plans.Exists(x => x.Activity.ToUpper() == activity.ToUpper());
            if (!removeDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                RemovePlan(ref calendar);
            }
            else if (!planActivity)
            {
                Console.WriteLine("There is no such activity planned, try again.");
                Console.ReadLine();
                RemovePlan(ref calendar);
            }
        }
    }
}
