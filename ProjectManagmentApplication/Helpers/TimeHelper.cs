using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Helpers
{
    public static class TimeHelper
    {
        public static string TimeAgo(DateTime date)
        {
            var timeSpan = DateTime.Now.Subtract(date);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return string.Format("{0} minutes ago", timeSpan.Minutes);
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                return String.Format("{0} hours ago", timeSpan.Hours);
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                return String.Format("{0} days ago", timeSpan.Days);
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                return String.Format("{0} months ago", timeSpan.Days / 30);
            }
            else
            {
                return String.Format("{0} years ago", timeSpan.Days / 365);
            }
        }
    }
}