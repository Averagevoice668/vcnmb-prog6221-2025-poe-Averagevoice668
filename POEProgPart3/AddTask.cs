using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public class AddTask//(Troelsen, A. and Japikse, P., 2022)
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? Reminder { get; set; } = null;//(MicrosoftLearn, 2025)
        public bool IsComplete { get; set; } = false;

        public override string ToString()
        {
            string status = IsComplete ? "[Completed]" : "[Pending]";
            string reminderStr = Reminder.HasValue ? $" Reminder: {Reminder.Value:g}" : "";
            return $"{status} {Title} - {Description}{reminderStr}";
        }
    }
}
/*MicrosoftLearn, 2025. DateTime.Now Property[online] Available at:
   < https://learn.microsoft.com/en-us/dotnet/api/system.datetime.now?view=net-9.0 >
    [Accessed 25 June 2025]
Troelsen, A. and Japikse, P., 2022.Pro C# 10 with .NET 6. 11th ed. New York: Apress Media, LLC.
*/