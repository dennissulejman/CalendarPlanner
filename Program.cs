using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


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
                    CreateCalendarPlanner();
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
        static void CreateCalendarPlanner()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            FileStream createNewPlanner = File.Create(path + @"\Calendar.csv");
            createNewPlanner.Close();
        }

        static void GetCalendarPlanner(ref Calendar calendar)
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
                Console.WriteLine(plan.Display);
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
                Console.WriteLine("Until which date and time does it last?(dd/mm/yy hh:mm)");
                string endTime = Console.ReadLine();
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
                string activity = Console.ReadLine();
                calendar.RemovePlanEntry(removeDate, activity);
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
        public static void EditPlan(ref Calendar calendar)
        {
            GetCalendarPlanner(ref calendar);
            bool edit = true;
            while (edit)
            {
                Console.Clear();

                Console.WriteLine("When is the plan you would like to edit?");
                string removeDate = Console.ReadLine();
                Console.WriteLine("Which plan would you like to remove?");
                string activity = Console.ReadLine();
                calendar.RemovePlanEntry(removeDate, activity);
                Console.WriteLine("Are you done removing plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    edit = true;
                    Console.Clear();
                }
                else if (response == "y")
                {
                    edit = false;
                }
                SaveCalendarPlanner(ref calendar);
            }
        }
    }
}
