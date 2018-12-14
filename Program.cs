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
            bool mainMenu = true;
            while (mainMenu)
            {
                mainMenu = MainMenu();
            }
        }
        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome! What would you like to do?");
            Console.WriteLine("Type the corresponding number then press enter to execute:");
            Console.WriteLine("1: Create a new calendar-planner.");
            Console.WriteLine("2: Open and view/edit your existing calendar-planner.");
            Console.WriteLine("3: Edit your existing calendar-planner.");
            Console.WriteLine();
            Console.WriteLine("4: Close the application.");
            string response = Console.ReadLine();
            if (response == "1")
            {
                CreatePlanner();
                return true;
            }
            else if (response == "2")
            {
                ReadPlanner();
                return true;
            }
            else if (response == "3")
            {
                EditPlanner();
                return true;
            }
            else if (response == "4")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void CreatePlanner()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            Calendar calendar = new Calendar();
            Dictionary<DateTime, string> myCalendar = calendar.GetCalendar();
            File.ReadAllLines(path + @"\Calendar.csv");
            Console.Clear();
            bool add = true;
            while (add)
            {

                Console.WriteLine("What are you planning to do?");
                string Activity = Console.ReadLine();
                Console.WriteLine("When are you planning to do it?(dd/mm/yy/)");
                string Date = Console.ReadLine();

                calendar.calendar.Add(DateTime.Parse(Date), Activity);
                Console.WriteLine("Are you done planning?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    add = true;
                }
                else if (response == "y")
                {
                    add = false;
                    Console.Clear();
                    Console.WriteLine("Here are your plans for the year:");
                    Console.WriteLine();
                    StringBuilder Plans = new StringBuilder();
                    Plans.AppendLine("My Calendar:");
                    Plans.AppendLine();
                    var dates = myCalendar.Select(p => p.Key + "," + p.Value.ToString());
                    foreach (KeyValuePair<DateTime, string> p in myCalendar.OrderBy(p => p.Key))
                    {
                        Console.WriteLine($"{p.Key}: {p.Value}");
                        Plans.AppendLine($"{p.Key}: {p.Value}".ToString());
                        File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());
                    }
                    Console.ReadLine();
                }

            }
        }
        public static void ReadPlanner()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.Clear();
            string read = File.ReadAllText(path + @"\Calendar.csv");
            Console.Write(read);
            Console.WriteLine();
            Console.WriteLine("Would you like to edit your calendar-planner?(y/n)");
            string response = Console.ReadLine();
            if (response == "y")
            {
                EditPlanner();
            }
        }
        public static bool EditPlanner()
        {
            Console.Clear();
            Console.WriteLine("How would you like to edit your calendar-planer?");
            Console.WriteLine("Type the corresponding number then press enter to execute:");
            Console.WriteLine("1: Add new plans.");
            Console.WriteLine("2: Edit existing plans.");
            Console.WriteLine("3: Remove existing plans.");
            Console.WriteLine();
            Console.WriteLine("4: Return to main menu.");
            string response = Console.ReadLine();
            if (response == "1")
            {
                AddPlan();
                return true;
            }
            else if (response == "2")
            {
                EditPlan();
                return true;
            }
            else if (response == "3")
            {
                RemovePlan();
                return true;
            }
            else if (response == "4")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void AddPlan()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.Clear();
            StreamReader calendarReader = new StreamReader(path + @"\Calendar.csv");
            Calendar calendar = new Calendar();
            Dictionary<DateTime, string> myCalendar = calendar.GetCalendar();
            using (calendarReader)
            {
                calendarReader.ReadLine();
                calendarReader.ReadLine();
                while (calendarReader.Peek() != -1)
                {
                    string line = calendarReader.ReadLine();
                    string[] header = line.Split(':');
                    string[] columns = line.Split(' ');

                    myCalendar.Add(DateTime.Parse(columns[0]), columns[3]);
                }
            }
            calendarReader.Close();
            Console.Clear();
            bool add = true;
            while (add)
            {

                Console.WriteLine("What are you planning to do?");
                string Activity = Console.ReadLine();
                Console.WriteLine("When are you planning to do it?(dd/mm/yy/)");
                string Date = Console.ReadLine();

                myCalendar.Add(DateTime.Parse(Date), Activity);
                Console.WriteLine("Are you done adding plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    add = true;
                }
                else if (response == "y")
                {
                    add = false;
                    Console.Clear();
                    Console.WriteLine("Here are your new plans for the year:");
                    Console.WriteLine();
                    StringBuilder Plans = new StringBuilder();
                    Plans.AppendLine("My Calendar:");
                    Plans.AppendLine();
                    var dates = myCalendar.Select(p => p.Key + p.Value.ToString());
                    foreach (KeyValuePair<DateTime, string> p in myCalendar.OrderBy(p => p.Key))
                    {
                        Console.WriteLine($"{p.Key}: {p.Value}");
                        Plans.AppendLine($"{p.Key}: {p.Value}".ToString());
                        File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());
                    }
                    Console.ReadLine();
                }

            }
        }
        public static void RemovePlan()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.Clear();
            StreamReader calendarReader = new StreamReader(path + @"\Calendar.csv");
            Calendar calendar = new Calendar();
            Dictionary<DateTime, string> myCalendar = calendar.GetCalendar();
            using (calendarReader)
            {
                calendarReader.ReadLine();
                calendarReader.ReadLine();
                while (calendarReader.Peek() != -1)
                {
                    string line = calendarReader.ReadLine();
                    string[] header = line.Split(':');
                    string[] columns = line.Split(' ');

                    myCalendar.Add(DateTime.Parse(columns[0]), columns[3]);
                }
            }
            calendarReader.Close();
            Console.Clear();
            bool remove = true;
            while (remove)
            {

                Console.WriteLine("When is the plan you would like to remove??(dd/mm/yy/)");
                string Date = Console.ReadLine();

                myCalendar.Remove(DateTime.Parse(Date));
                Console.WriteLine("Are you done removing plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    remove = true;
                }
                else if (response == "y")
                {
                    remove = false;
                    Console.Clear();
                    Console.WriteLine("Here are your new plans for the year:");
                    Console.WriteLine();
                    StringBuilder Plans = new StringBuilder();
                    Plans.AppendLine("My Calendar:");
                    Plans.AppendLine();
                    var dates = myCalendar.Select(p => p.Key + p.Value.ToString());
                    foreach (KeyValuePair<DateTime, string> p in myCalendar.OrderBy(p => p.Key))
                    {
                        Console.WriteLine($"{p.Key}: {p.Value}");
                        Plans.AppendLine($"{p.Key}: {p.Value}".ToString());
                        File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());
                    }
                    Console.ReadLine();
                }

            }
        }
        public static void EditPlan()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.Clear();
            StreamReader calendarReader = new StreamReader(path + @"\Calendar.csv");
            Calendar calendar = new Calendar();
            Dictionary<DateTime, string> myCalendar = calendar.GetCalendar();
            using (calendarReader)
            {
                calendarReader.ReadLine();
                calendarReader.ReadLine();
                while (calendarReader.Peek() != -1)
                {
                    string line = calendarReader.ReadLine();
                    string[] header = line.Split(':');
                    string[] columns = line.Split(' ');

                    myCalendar.Add(DateTime.Parse(columns[0]), columns[3]);
                }
            }
            calendarReader.Close();
            Console.Clear();
            bool edit = true;
            while (edit)
            {

                Console.WriteLine("When is the plan you would like to edit?(dd/mm/yy/)");
                string Date = Console.ReadLine();
                Console.WriteLine("What are you planning to do instead?");
                string Activity = Console.ReadLine();

                myCalendar[DateTime.Parse(Date)] = Activity;
                Console.WriteLine("Are you done editing plans?(y/n)");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    edit = true;
                }
                else if (response == "y")
                {
                    edit = false;
                    Console.Clear();
                    Console.WriteLine("Here are your new plans for the year:");
                    Console.WriteLine();
                    StringBuilder Plans = new StringBuilder();
                    Plans.AppendLine("My Calendar:");
                    Plans.AppendLine();
                    var dates = myCalendar.Select(p => p.Key + p.Value.ToString());
                    foreach (KeyValuePair<DateTime, string> p in myCalendar.OrderBy(p => p.Key))
                    {
                        Console.WriteLine($"{p.Key}: {p.Value}");
                        Plans.AppendLine($"{p.Key}: {p.Value}".ToString());
                        File.WriteAllText(path + @"\Calendar.csv", Plans.ToString());
                    }
                    Console.ReadLine();
                }

            }
        }
    }
}