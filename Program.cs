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
using System.Security.Cryptography;

namespace CalendarPlanner
{   
    class Program
    {
        static void Main(string[] args)
        {
        CalendarContext db = new CalendarContext();
            Console.WriteLine("Hi and Welcome to My Calendar-Planner Application!");
            Console.WriteLine();
            Console.WriteLine("Proceed by typing the corresponding number then");
            Console.WriteLine("press enter to execute the action:");
            Console.WriteLine("1: Log in to your account.");
            Console.WriteLine("2: Create new user.");
            Console.WriteLine("3: Remove existing user.");
            Console.WriteLine();
            Console.WriteLine("4: Close the application");
            string response = Console.ReadLine();
            if (response == "1")
            {
                UserLogin(ref db);
            }
            else if (response == "2")
            {
                UserCreation(ref db);
            }
            else if (response == "3")
            {
                UserDeletion(ref db);
            }
            else if (response == "4")
            {
                Environment.Exit(0);
            }
            while (true)
            {
                
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type the corresponding number then press enter to execute:");
                Console.WriteLine("1: Create a new calendar-planner.");
                Console.WriteLine("2: Add Plans.");
                Console.WriteLine("3: Remove Plans.");
                Console.WriteLine("4: Display your existing calendar-planner.");
                Console.WriteLine();
                Console.WriteLine("5: Close the application.");
                response = Console.ReadLine();
                if (response == "1")
                {
                    CreateCalendarPlanner(ref db);
                    PlanAdd(ref db);
                }
                else if (response == "2")
                {
                    PlanAdd(ref db);
                }
                else if (response == "3")
                {
                    PlanRemove(ref db);
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
                //if (db.Calendars.Find(c => c.Owner == owner))
                /*{
                    Console.WriteLine("Found existing Calendar-Planner.");
                    Console.WriteLine("Would you like to replace your current Planner?(y/n)");
                    string response = Console.ReadLine();
                    if (response == "y")
                    {
                        /*db.Calendars.Update(c => c.)
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
                }*/
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
        
        public static void PlanAdd(ref CalendarContext db)
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

        public static void PlanRemove(ref CalendarContext db)
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
                PlanAdd(ref db);
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
                PlanRemove(ref db);
            }
            else if (!planActivity)
            {
                Console.WriteLine("There is no such activity planned, try again.");
                Console.ReadLine();
                PlanRemove(ref db);
            }
        }
        public static void UserLogin(ref CalendarContext db)
        {
            Console.Clear();
            Console.WriteLine("Welcome! Please enter your username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string passWord = Console.ReadLine();
            UserGetHashedPassword(ref db, userName, passWord);
        }
        public static void UserCreation(ref CalendarContext db)
        {
            bool passwordConfirmation = true;
            while (passwordConfirmation)
            {
                Console.Clear();
                Console.WriteLine("Please enter your desired username:");
                string userName = Console.ReadLine();
                Console.WriteLine("Please enter your desired password:");
                string passWord = UserPasswordMasking();
                Console.WriteLine("Please confirm your desired password:");
                string passWordConfirm = UserPasswordMasking();
                if (passWord == passWordConfirm)
                {                  
                    string hashedPassword = UserPasswordHashing(passWordConfirm);
                    UserAdd(ref db, userName, hashedPassword);                   
                    passwordConfirmation = false;
                }
                else if (passWord != passWordConfirm)
                {
                    Console.WriteLine("The password was not confirmed!");
                    Console.WriteLine("Press enter to try again.");
                    Console.ReadLine();
                    passwordConfirmation = true;
                }
            }
        }
        public static void UserDeletion(ref CalendarContext db)
        {
            Console.WriteLine("Please enter your desired username:");
            Console.WriteLine("Please enter your desired password:");
            Console.WriteLine("Please confirm your desired password:");
            Console.WriteLine("This action will delete your account");
            Console.WriteLine("Would you like to proceed? (y/n)");
        }
        public static void UserLogout(ref CalendarContext db)
        {

        }
        public static void UserAdd(ref CalendarContext db, string userName, string hashedPassword)
        {
            //Remove Database-tracking when loop restarts.
            try
            {
                db.Users.Add(new User { Username = userName,
                    Password = hashedPassword,
                    Calendar = new Calendar { Username = userName } });
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Username already taken!");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                db.Entry(entity: db).State = EntityState.Detached;
                UserCreation(ref db);
                
            } 
        }
        public static string UserPasswordMasking()
        {
            ConsoleKeyInfo key;
            string passWord = null;
            do
            {                
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    Console.Write("*");
                    passWord += key.KeyChar;
                }
                else if (key.Key == ConsoleKey.Backspace && passWord.Length > 0)
                {
                    passWord.Substring(0, passWord.Length - 1);
                    Console.Write("\b \b");
                }                               
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return passWord;    
        }
        public static string UserPasswordHashing(string passWord)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(passWord, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }       
        public static void UserGetHashedPassword(ref CalendarContext db, string userName, string passWord)
        {
            // Fetch the stored value
            var savedPasswordHash = db.Users.Where(u => u.Username == userName).SingleOrDefault().Password;
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(passWord, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            // Compare the results 
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] == hash[i])
                {
                    UserAuthentication(ref db, userName, savedPasswordHash);
                }
                else if (hashBytes[i + 16] != hash[i])
                {
                    Console.WriteLine("Password is incorrect!");
                    Console.WriteLine("Press enter to try again.");
                    Console.ReadLine();                    
                    UserLogin(ref db);
                    break;
                }
        }
        public static void UserAuthentication(ref CalendarContext db, string userName, string savedPasswordHash)
        {          
            try
            {
                var existingUser = db.Users.Single(u => u.Username == userName && u.Password == savedPasswordHash);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Wrong information! Try again.");
                Console.ReadLine();
                UserLogin(ref db);
            }
        }
    }
}