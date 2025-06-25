using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public static class ActivityLog
    {
        private static List<string> logEntries = new List<string>();

        // Add a new log entry
        public static void AddLog(string entry)
        {
            logEntries.Add($"{DateTime.Now:g}: {entry}");
        }

        // Get recent logs (e.g., last 5 or 10)
        public static List<string> ShowLog(int numberOfEntries = 5)
        {
            return logEntries
                .TakeLast(numberOfEntries)
                .ToList();
        }

        // Optionally clear the log (if needed)
        public static void ClearLog()
        {
            logEntries.Clear();
        }
    }
}
