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
using Microsoft.EntityFrameworkCore;


namespace CalendarPlanner
{   
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                CalendarContext db = new CalendarContext();
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
                    CreateCalendarPlanner(ref db);
                    AddPlan(ref db);
                }
                else if (response == "2")
                {
                    AddPlan(ref db);
                }
                else if (response == "3")
                {
                    RemovePlan(ref db);
                }
                else if (response == "4")
                {
                    DisplayCalendarPlanner(ref db);
                }
                else if (response == "5")
                {
                    Environment.Exit(0);
                }
                Console.ReadLine();
            }
        }
        public static void CreateCalendarPlanner(ref CalendarContext db)
        {
            //Method has to be updated to use the Database.

            bool detectCalendarPlanner = true;
            while (detectCalendarPlanner)
            {
                string owner = Console.ReadLine();
                if (db.Calendars.Any(c => c.Owner == owner))
                {
                    Console.WriteLine("Found existing Calendar-Planner.");
                    Console.WriteLine("Would you like to replace your current Planner?(y/n)");
                    string response = Console.ReadLine();
                    if (response == "y")
                    {
                        /*db.Calendars.Update(c => c.)
                        createNewPlanner.Close();
                        detectCalendarPlanner = false;*/
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
                    /*createNewPlanner = File.Create(path + @"\Calendar.csv");
                    createNewPlanner.Close();
                    detectCalendarPlanner = false;*/
                }
            }
        }

        public static void GetCalendarPlanner(ref Calendar calendar)
        {
            //Method has to be updated to use the Database.
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

        public static void DisplayCalendarPlanner(ref CalendarContext db)
        {
            Console.WriteLine("These are your current plans:");
            Console.WriteLine();
            foreach (Plan plan in db.Plans)
            {
                Console.WriteLine($"{plan.Activity}, {plan.StartTime:dd MMM yyyy HH:mm}, {plan.EndTime:dd MMM yyyy HH:mm}");
            }
        }

        public static void SaveCalendarPlanner(ref Calendar calendar)
        {
            //This method will either be updated or removed according to the Database.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.WriteLine("Here are your new plans for the year:");
            Console.WriteLine();
            StringBuilder Plans = new StringBuilder();
            Plans.AppendLine("My Calendar:");
            Plans.AppendLine();
            foreach (Plan plan in calendar.Plans.OrderBy(v => v.StartTime))
            {
                /*Console.WriteLine(plan.Display, Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(plan.Activity));
                string dsa = (plan.Display);
                Plans.AppendLine(plan.Display);
                File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());*/
            }
        }
        
        public static void AddPlan(ref CalendarContext db)
        {

            bool add = true;
            while (add)
            {
                Console.Clear();
                Console.WriteLine("What are you planning to do?");
                string activity = Console.ReadLine();
                Console.WriteLine("Which date and time does it start?(dd/mm/yy hh:mm)");
                string startTime = Console.ReadLine();
                ValidateAddDateFormat(ref db, startTime);
                Console.WriteLine("Until which date and time does it last?(dd/mm/yy hh:mm)");
                string endTime = Console.ReadLine();
                ValidateAddDateFormat(ref db, endTime);
                Calendar calendar = new Calendar { CalendarId = 1 };
                calendar.AddPlanEntry(startTime, endTime, activity);
                db.Attach(calendar);
                db.Entry(calendar).State = EntityState.Detached;
                db.SaveChanges();
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
            }
        }

        public static void RemovePlan(ref CalendarContext db)
        {
            bool remove = true;
            while (remove)
            {
                Console.Clear();
                Console.WriteLine("When is the plan you would like to remove?");
                string removeDate = Console.ReadLine();
                Console.WriteLine("Which plan would you like to remove?");
                string activity = Console.ReadLine().ToUpper();
                ValidateRemoveDateFormat(ref db, removeDate, activity);
                var calendar = db.Calendars.Single(c => c.CalendarId == 1);
                var plans = db.Plans.Where(p => p.StartTime == DateTime.Parse(removeDate) && p.Activity.ToUpper() == activity.ToUpper());
                foreach (var plan in plans)
                {
                    db.Plans.Remove(plan);
                }
                db.SaveChanges();
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
            }
        }

        public static void ValidateAddDateFormat(ref CalendarContext db, string date)
        {
            bool addDateFormat = true;
            addDateFormat &= DateTime.TryParse(date, out DateTime result);

            if (!addDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                AddPlan(ref db);
            }
        }
        
        public static void ValidateRemoveDateFormat(ref CalendarContext db, string date, string activity)
        {
            bool removeDateFormat = true;
            bool planActivity = true;
            removeDateFormat &= DateTime.TryParse(date, out DateTime result);
            planActivity &= db.Plans.Any(x => x.Activity.ToUpper() == activity.ToUpper());
            if (!removeDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                RemovePlan(ref db);
            }
            else if (!planActivity)
            {
                Console.WriteLine("There is no such activity planned, try again.");
                Console.ReadLine();
                RemovePlan(ref db);
            }
        }
    }
}