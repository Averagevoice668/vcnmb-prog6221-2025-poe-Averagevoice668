using System.Speech.Synthesis;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
namespace POEProgPart3
{
    public partial class Form1 : Form
    {
        private static SaveData userData = new SaveData();
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private bool firstLoad = true;
        private bool nameEntered = false;
        private bool topicEntered = false;
        private List<AddTask> tasks = new List<AddTask>();

        public Form1()
        {
            InitializeComponent();
            //Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)//(Troelsen, A. and Japikse, P., 2022)
        {
            try
            {
                AppendMessage(rbtOutput, "Chatbot", "Welcome to CourtoMates! Your personal cybersecurity chatbot.");
                AppendMessage(rbtOutput, "Chatbot", "Please enter your name:");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Form1_Load: " + ex.Message);
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)//(Troelsen, A. and Japikse, P., 2022)
        {
            string userMessage = txtInput.Text.Trim();

            if (!string.IsNullOrEmpty(userMessage))
            {
                AppendMessage(rbtOutput, "User", userMessage);

                if (!nameEntered)
                {
                    ChatBot.SetUserName(userMessage);
                    nameEntered = true;

                    AppendMessage(rbtOutput, "Chatbot", $"Hello {userMessage}! What is your favourite cybersecurity topic? (Password Safety," +
                        $" Phishing, Safe Browsing)");
                }
                else if (!topicEntered)
                {
                    bool validTopic = ChatBot.SetFavouriteTopic(userMessage);
                    if (validTopic)
                    {
                        topicEntered = true;
                        AppendMessage(rbtOutput, "Chatbot", $"Great! I'll remember that you like {userMessage.ToLower()}.");
                        AppendMessage(rbtOutput, "Chatbot", "You can now ask me a question, or type one of the following commands:\n" +
                            "1 'add task' - to add a cybersecurity task\n" +
                            "2 'view tasks' - to view your tasks\n" +
                            "3 'delete task [number]' - to delete a task\n" +
                            "4 'complete task [number]' - to mark a task complete\n" +
                            "5 'start quiz' - to test your cybersecurity knowledge\n" +
                            "6 'exit' - to quit.");
                    }
                    else
                    {
                        AppendMessage(rbtOutput, "Chatbot", "Sorry, I didn't recognize that topic. Please enter Password Safety, Phishing, or Safe Browsing.");
                    }
                }
                else
                {
                    // If the quiz is in progress, handle quiz answers
                    if (ChatBot.IsQuizInProgress())
                    {
                        ChatBot.HandleQuizAnswer(userMessage, rbtOutput);
                    }
                    // Start quiz when user types 'start quiz'
                    else if (userMessage.ToLower() == "start quiz")
                    {
                        ChatBot.StartQuiz(rbtOutput);
                    }
                    // Handle adding tasks
                    else if (ChatBot.IsAddingTask())
                    {
                        ChatBot.ContinueAddTask(userMessage, rbtOutput, tasks);
                    }
                    // Handle task commands
                    else if (userMessage.ToLower().StartsWith("add task"))
                    {
                        ChatBot.StartAddTask(rbtOutput);
                    }
                    else if (userMessage.ToLower() == "view tasks")
                    {
                        ChatBot.ViewTasks(rbtOutput, tasks);
                    }
                    else if (userMessage.ToLower().StartsWith("delete task"))
                    {
                        ChatBot.HandleDeleteTask(userMessage, rbtOutput, tasks);
                    }
                    else if (userMessage.ToLower().StartsWith("complete task"))
                    {
                        ChatBot.HandleCompleteTask(userMessage, rbtOutput, tasks);
                    }
                    else if (userMessage.ToLower() == "show log")
                    {
                        ChatBot.ShowLog(rbtOutput);
                    }
                    // Normal chat or cybersecurity questions
                    else
                    {
                        string response = ChatBot.StartChat(userMessage, rbtOutput, tasks);
                        if (response == "exit")
                        {
                            AppendMessage(rbtOutput, "Chatbot", $"Stay safe, goodbye!");
                            btnSubmit.Enabled = false;
                            txtInput.Enabled = false;
                        }
                    }
                }

                txtInput.Clear();
            }
        }

        public void rbtOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnStart_Click(object sender, EventArgs e)//(Troelsen, A. and Japikse, P., 2022)
        {
            btnStart.Enabled = false;

            Welcome.PlayWelcome();
            await Task.Delay(2000);
            AsciiArt.DisplayAsciiLogo(rbtOutput);
            await Task.Delay(2000);
            AppendMessage(rbtOutput, "Chatbot", "Welcome to CourtoMates! Your personal cybersecurity chatbot.");
            AppendMessage(rbtOutput, "Chatbot", "Please enter your name:");

        }

        private static void AppendMessage(RichTextBox conversationBox, string sender, string message)//(Troelsen, A. and Japikse, P., 2022)
        {
            conversationBox.SelectionStart = conversationBox.TextLength;
            conversationBox.SelectionLength = 0;
            conversationBox.SelectionColor = sender == "Chatbot" ? Color.Blue : Color.Green;
            conversationBox.AppendText($"{sender}: {message}\n");
            conversationBox.SelectionColor = Color.White;
            conversationBox.ScrollToCaret();

            if (sender == "Chatbot")
            {
                synth.SpeakAsyncCancelAll();
                synth.SpeakAsync(message);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
/*
Troelsen, A. and Japikse, P., 2022.Pro C# 10 with .NET 6. 11th ed. New York: Apress Media, LLC.
*/