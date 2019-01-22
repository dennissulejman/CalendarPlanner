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
            while (true)
            {
                CalendarContext db = new CalendarContext();
                Console.Clear();
                Console.WriteLine("Hi and Welcome to the Calendar-Planner Application!");
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
                    PlannerMenu(ref db, UserLogin(ref db));
                }
                else if (response == "2")
                {                   
                    PlannerMenu(ref db, UserCreation(ref db));
                }
                else if (response == "3")
                {
                    UserDeletion(ref db);
                }
                else if (response == "4")
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void PlannerMenu(ref CalendarContext db, string userName)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type the corresponding number then press enter to execute:");
                Console.WriteLine("1: Add Plans.");
                Console.WriteLine("2: Remove Plans.");
                Console.WriteLine("3: Display your existing calendar-planner.");
                Console.WriteLine("4: Log out.");
                Console.WriteLine();
                Console.WriteLine("5: Close the application.");
                string response = Console.ReadLine();
                if (response == "1")
                {
                    PlanAdd(ref db, userName);
                }
                else if (response == "2")
                {
                    PlanRemove(ref db, userName);
                }
                else if (response == "3")
                {
                    db.DisplayPlans(userName);
                }
                else if (response == "4")
                {
                    break;
                }
                else if (response == "5")
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void PlanAdd(ref CalendarContext db, string userName)
        {
            bool add = true;
            while (add)
            {
                Console.Clear();
                Console.WriteLine("What are you planning to do?");
                string activity = Console.ReadLine();
                Console.WriteLine("Which date and time does it start?(dd/mm/yy hh:mm)");
                string startTime = Console.ReadLine();
                ValidateAddDateFormat(ref db, startTime, userName);
                Console.WriteLine("Until which date and time does it last?(dd/mm/yy hh:mm)");
                string endTime = Console.ReadLine();
                ValidateAddDateFormat(ref db, endTime, userName);
                db.AddPlanEntry(startTime, endTime, activity, userName);
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

        public static void PlanRemove(ref CalendarContext db, string userName)
        {
            bool remove = true;
            while (remove)
            {
                Console.Clear();
                Console.WriteLine("When is the plan you would like to remove?");
                string removeDate = Console.ReadLine();
                Console.WriteLine("Which plan would you like to remove?");
                string activity = Console.ReadLine();
                ValidateRemoveDateFormat(ref db, removeDate, activity, userName);
                db.RemovePlanEntry(removeDate, activity, userName);
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

        public static void ValidateAddDateFormat(ref CalendarContext db, string date, string userName)
        {
            bool addDateFormat = true;
            addDateFormat &= DateTime.TryParse(date, out DateTime result);
            if (!addDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                PlanAdd(ref db, userName);
            }
        }

        public static void ValidateRemoveDateFormat(ref CalendarContext db, string date, string activity, string userName)
        {
            bool removeDateFormat = true;
            bool planActivity = true;
            removeDateFormat &= DateTime.TryParse(date, out DateTime result);
            planActivity &= db.Plans.Any(x => x.Activity.ToUpper() == activity.ToUpper());
            if (!removeDateFormat)
            {
                Console.WriteLine("This is not a valid date format, try again.");
                Console.ReadLine();
                PlanRemove(ref db, userName);
            }
            else if (!planActivity)
            {
                Console.WriteLine("There is no such activity planned, try again.");
                Console.ReadLine();
                PlanRemove(ref db, userName);
            }
        }

        public static string UserLogin(ref CalendarContext db)
        {
            Console.Clear();
            Console.WriteLine("Welcome! Please enter your username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string passWord = UserPasswordMasking();
            UserGetHashedPassword(ref db, userName, passWord);
            return userName;
        }

        public static string UserCreation(ref CalendarContext db)
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
                    return userName;
                }
                else if (passWord != passWordConfirm)
                {
                    Console.WriteLine("The password was not confirmed!");
                    Console.WriteLine("Press enter to try again.");
                    Console.ReadLine();
                    UserCreation(ref db);
                }
            }
            return null;
        }

        public static void UserDeletion(ref CalendarContext db)
        {
            bool passwordConfirmation = true;
            while (passwordConfirmation)
            {
                Console.Clear();
                Console.WriteLine("Please enter your username:");
                string userName = Console.ReadLine();
                Console.WriteLine("Please enter your password:");
                string passWord = UserPasswordMasking();
                Console.WriteLine("Please confirm your password:");
                string passWordConfirm = UserPasswordMasking();
                if (passWord == passWordConfirm)
                {
                    Console.WriteLine("This action will delete your account");
                    Console.WriteLine("Would you like to proceed? (y/n)");
                    string response = Console.ReadLine();
                    if (response == "y")
                    {
                        string hashedPassword = UserPasswordHashing(passWordConfirm);
                        UserRemove(ref db, userName, hashedPassword);
                        break;
                    }
                    else if (response == "n")
                    {
                        break;
                    }
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

        public static void UserAdd(ref CalendarContext db, string userName, string hashedPassword)
        {
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
                db.DetachAllEntities();
                UserCreation(ref db);                
            }
        }

        public static void UserRemove(ref CalendarContext db, string userName, string hashedPassword)
        {
            try
            {
                db.Users.Remove(new User
                {
                    Username = userName,
                    Password = hashedPassword,
                });
                db.SaveChanges();
                db.Calendars.Remove(new Calendar
                {
                    CalendarId = db.GetCalendarId(userName)
                });
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Username not found!");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                db.DetachAllEntities();
                UserDeletion(ref db);
            }
        }

        public static string UserPasswordMasking()
        {
            ConsoleKeyInfo key;
            string passWord = string.Empty;
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
            UserAuthentication(ref db, userName, passWord);
            string savedPasswordHash = db.Users.Where(u => u.Username == userName)
            .SingleOrDefault().Password;

            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(passWord, salt, 10000);
            ConvertToHashedPassword(ref db, passWord, salt);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] == hash[i])
                {
                    UserAuthentication(ref db, userName, passWord);
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

        public static void UserAuthentication(ref CalendarContext db, string userName, string passWord)
        {
            try
            {
                string existingUser = db.Users.Single(u => u.Username == userName).ToString();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("User does not exist!");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();                
                UserLogin(ref db);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("User does not exist!");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                UserLogin(ref db);
            }
            string savedPasswordHash = db.Users.Where(u => u.Username == userName)
                                    .SingleOrDefault().Password;
        }

        public static void ConvertToHashedPassword(ref CalendarContext db, string passWord, byte[] salt)
        {
            try
            {
                var pbkdf2 = new Rfc2898DeriveBytes(passWord, salt, 10000);
            }
            catch (Exception)
            {
                Console.WriteLine("Password is incorrect!");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                UserLogin(ref db);
            }
        }
    }
}