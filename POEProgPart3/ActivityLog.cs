using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public static class ActivityLog//(Troelsen, A. and Japikse, P., 2022)
    {
        private static List<string> logEntries = new List<string>();

        // Add a new log entry
        public static void AddLog(string entry)
        {
            logEntries.Add($"{DateTime.Now:g}: {entry}");//(MicrosoftLearn, 2025)
        }

        // Get recent logs (e.g., last 5 or 10)
        public static List<string> ShowLog(int numberOfEntries = 5)//(Troelsen, A. and Japikse, P., 2022)
        {
            return logEntries
                .TakeLast(numberOfEntries)
                .ToList();
        }

        // Optionally clear the log (if needed)
        public static void ClearLog()//(Troelsen, A. and Japikse, P., 2022)
        {
            logEntries.Clear();
        }
    }
}
/*MicrosoftLearn, 2025. DateTime.Now Property[online] Available at:
   < https://learn.microsoft.com/en-us/dotnet/api/system.datetime.now?view=net-9.0 >
    [Accessed 25 June 2025]
Troelsen, A. and Japikse, P., 2022.Pro C# 10 with .NET 6. 11th ed. New York: Apress Media, LLC.
*/