using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeBridgeSoftware.CoreApps;

namespace VIPDayCareCenters.Helpers
{
    public class VIPHelpers
    {

        public static string calculateAge(DateTime birthday)
        {
            return cGeneral.Age(birthday).ToString();
        }

        public static string formatScheduledDates(string excludedDates)
        {

            string scheduledDays = string.Empty;

            excludedDates = excludedDates.Trim();

            if (string.IsNullOrEmpty(excludedDates))
            {
                scheduledDays = "Mon,Tue,Wed,Thu,Fri";
                return scheduledDays;
            }

            for (var d = DayOfWeek.Monday; d < DayOfWeek.Saturday; d++)
            {

                var dayNum = ((int)d).ToString();

                if (!excludedDates.Contains(dayNum))
                {
                    scheduledDays += d.ToString().Substring(0, 3) + ",";
                }

            }

            return (scheduledDays.Length > 0) ? scheduledDays.Remove(scheduledDays.Length - 1, 1) : scheduledDays;

        }
    }
}