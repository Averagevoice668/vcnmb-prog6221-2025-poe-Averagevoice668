using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public class AddTask
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? Reminder { get; set; } = null;
        public bool IsComplete { get; set; } = false;

        public override string ToString()
        {
            string status = IsComplete ? "[Completed]" : "[Pending]";
            string reminderStr = Reminder.HasValue ? $" Reminder: {Reminder.Value:g}" : "";
            return $"{status} {Title} - {Description}{reminderStr}";
        }
    }
}
