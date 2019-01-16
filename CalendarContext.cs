
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarPlanner
{

    public class CalendarContext : DbContext
    {
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Plan> Plans { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["CalendarContext"]
            .ConnectionString, options => options.EnableRetryOnFailure())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public void DetachAllEntities()
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)

                .ToList();
            foreach (var entry in changedEntries)
                entry.State = EntityState.Detached;
        }

        public int GetCalendarId(string userName)
        {
            int calendarId = Calendars.Where(c => c.Username == userName).Single().CalendarId;
            return calendarId;
        }

        public void DisplayPlans(string userName)
        {
            Console.WriteLine("These are your current plans:");
            Console.WriteLine();

            var plans = Plans.Where(p => EF.Property<int>(p, "CalendarId") == GetCalendarId(userName))
                        .OrderBy(p => p.StartTime);
            foreach (Plan plan in plans)
            {
                Console.WriteLine($"{plan.Activity}, {plan.StartTime:dd MMM yyyy HH:mm}, {plan.EndTime:dd MMM yyyy HH:mm}");
            }
            Console.ReadLine();
        }

        public void AddPlanEntry(string startTime, string endTime, string activity, string userName)
        {
            var calendar = new Calendar { CalendarId = GetCalendarId(userName) };
            var plan = new Plan
            {
                StartTime = DateTime.Parse(startTime),
                EndTime = DateTime.Parse(endTime),
                Activity = activity,
                Calendar = calendar
            };
            Attach(plan);
            SaveChanges();
            Entry(calendar).State = EntityState.Detached;
            Entry(plan).State = EntityState.Detached;
        }

        public void RemovePlanEntry(string removeDate, string activity, string userName)
        {
            var plansToRemove = Plans.Where(p => EF.Property<int>(p, "CalendarId") == GetCalendarId(userName)
                                && p.StartTime == DateTime.Parse(removeDate) 
                                && p.Activity.ToUpper() == activity.ToUpper());
            foreach (var plan in plansToRemove)
            {
                Plans.Remove(plan);
            }
            SaveChanges();
            DetachAllEntities();
        }
    }
}
