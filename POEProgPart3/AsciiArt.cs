using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public static class AsciiArt
    {
        public static void DisplayAsciiLogo(RichTextBox conversationBox)
        {

            string asciiLogo = @"
   ____                 _                        _           
  / ___|___  _   _ _ __| |_ ___  _ __ ___   __ _| |_ ___  ___ 
 | |   / _ \| | | | '__| __/ _ \| '_ ` _ \ / _` | __/ _ \/ __|
 | |__| (_) | |_| | |  | || (_) | | | | | | (_| | ||  __/\__ \
  \____\___/ \__,_|_|   \__\___/|_| |_| |_|\__,_|\__\___||___/
                                                                 
           Welcome to the Cybersecurity Awareness Bot
";
            conversationBox.SelectionFont = new Font("Courier New", 12, FontStyle.Bold);
            conversationBox.SelectionColor = Color.DarkRed;
            conversationBox.AppendText(asciiLogo + "\n\n");
            Thread.Sleep(500); // Adds a pause for effect
            //(Chand, 2025)
        }
    }
}
