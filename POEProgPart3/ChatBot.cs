using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace POEProgPart3
{
    public static class ChatBot
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static Random random = new Random();
        private static string lastTopic = ""; // Track last discussed topic
        private static bool expectingFollowUp = false; // Flag to track follow-up questions
        private static SaveData userData = new SaveData(); // Separate class for storing user data
        private static bool isAddingTask = false;
        private static int addTaskStep = 0;
        private static AddTask newTask;
        private static bool quizInProgress = false;
        private static int currentQuestionIndex = 0;
        private static int correctAnswers = 0;
        private static List<string> incorrectQuestions = new List<string>();

        private static Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>
        {
            { "password safety", new List<string>
                {
                    "Use a password manager to securely store your credentials.",
                    "Enable multi-factor authentication (MFA) for extra security.",
                    "Never use personal information in passwords, such as birthdays or names!"
                }
            },
            { "phishing", new List<string>
                {
                    "Phishing emails often create a sense of urgency—don't rush to act.",
                    "Be cautious of links, even if they appear legitimate.",
                    "Verify requests for sensitive information directly with the organization."
                }
            },
            { "safe browsing", new List<string>
                {
                    "Avoid clicking on pop-ups—many are used to trick users.",
                    "Regularly clear cookies and cached data to maintain privacy.",
                    "Use browser extensions that block malicious trackers and scripts."
                }
            }
        };

        // Sentiment detection responses
        private static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>
        {
            { "worried", "Cybersecurity can feel overwhelming, but you're not alone. Let's take it step by step." },
            { "frustrated", "I get it—security issues can be frustrating! Let’s break it down together." },
            { "curious", "Curiosity is great! Exploring cybersecurity will help you stay safe online." }
        };

        static ChatBot()
        {
            synth.SelectVoiceByHints(VoiceGender.Male); // Selects a male voice
            synth.Rate = 0;  // Normal speaking speed
            synth.Volume = 100; // Max volume
        }

        public static void SetUserName(string name)
        {
            userData.UserName = name.Trim();
        }

        public static bool SetFavouriteTopic(string topic)
        {
            topic = topic.Trim().ToLower();
            if (responses.ContainsKey(topic))
            {
                userData.FavouriteTopic = topic;
                return true;
            }
            return false;
        }

        public static string StartChat(string userInput, RichTextBox conversationBox, List<AddTask> tasks)
        {
            CheckReminders(conversationBox, tasks);

            if (string.IsNullOrWhiteSpace(userInput))
            {
                AppendMessage(conversationBox, "Chatbot", "You didn’t type anything. Please try again.");
                return "";
            }

            userInput = userInput.Trim().ToLower();

            if (userInput == "exit")
            {
                AppendMessage(conversationBox, "Chatbot", $"Stay safe, {userData.UserName}! Goodbye!");
                return "";
            }

            if (userInput.StartsWith("add task"))
            {
                StartAddTask(conversationBox);
                return "";
            }

            if (userInput == "view tasks")
            {
                if (tasks.Count == 0)
                {
                    AppendMessage(conversationBox, "Chatbot", "You have no tasks.");
                }
                else
                {
                    AppendMessage(conversationBox, "Chatbot", "Here are your tasks:");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        AppendMessage(conversationBox, "Chatbot", $"{i + 1}. {tasks[i]}");
                    }
                }
                return "";
            }

            if (userInput.StartsWith("delete task"))
            {
                HandleDeleteTask(userInput, conversationBox, tasks);
                return "";
            }

            if (userInput.StartsWith("complete task"))
            {
                HandleCompleteTask(userInput, conversationBox, tasks);
                return "";
            }

            foreach (var sentiment in sentimentResponses.Keys)
            {
                if (userInput.Contains(sentiment))
                {
                    AppendMessage(conversationBox, "Chatbot", sentimentResponses[sentiment]);
                    return userInput;
                }
            }

            if (expectingFollowUp && !string.IsNullOrEmpty(lastTopic) && responses.ContainsKey(lastTopic))
            {
                if (userInput.Contains("more details") || userInput.Contains("i do not understand"))
                {
                    AppendMessage(conversationBox, "Chatbot", SelectRandomResponse(responses[lastTopic]));
                    expectingFollowUp = false;
                    return userInput;
                }
                else
                {
                    AppendMessage(conversationBox, "Chatbot", "I'm not sure how to answer that. Try asking about cybersecurity topics.");
                    return userInput;
                }
            }

            if (userInput.Contains("how are you"))
                AppendMessage(conversationBox, "Chatbot", "I'm good thanks. What can I help you with?");
            else if (userInput.Contains("purpose"))
                AppendMessage(conversationBox, "Chatbot", "My purpose is to promote cybersecurity awareness.");
            else if (userInput.Contains("what can i ask"))
                AppendMessage(conversationBox, "Chatbot", "Ask me about password safety, phishing, or safe browsing.");
            else if (userInput.Contains("password safety"))
            {
                lastTopic = "password safety";
                expectingFollowUp = true;
                userData.FavouriteTopic = "password safety";
                AppendMessage(conversationBox, "Chatbot", "Use long, unique passwords with numbers and symbols. Don’t reuse them!");
                AppendMessage(conversationBox, "Chatbot", "If you need more details, type 'more details'.");
            }
            else if (userInput.Contains("phishing"))
            {
                lastTopic = "phishing";
                expectingFollowUp = true;
                userData.FavouriteTopic = "phishing";
                AppendMessage(conversationBox, "Chatbot", "Phishing tricks you into giving info via fake emails or websites. Stay alert!");
                AppendMessage(conversationBox, "Chatbot", "If you need more details, type 'more details'.");
            }
            else if (userInput.Contains("safe browsing"))
            {
                lastTopic = "safe browsing";
                expectingFollowUp = true;
                userData.FavouriteTopic = "safe browsing";
                AppendMessage(conversationBox, "Chatbot", "Stick to HTTPS websites, don’t click suspicious links, and update your browser.");
                AppendMessage(conversationBox, "Chatbot", "If you need more details, type 'more details'.");
            }
            else if (userInput.Contains("more details") && !string.IsNullOrEmpty(lastTopic))
            {
                AppendMessage(conversationBox, "Chatbot", SelectRandomResponse(responses[lastTopic]));
            }
            else if (userInput.Contains("my favourite"))
            {
                if (!string.IsNullOrEmpty(userData.FavouriteTopic))
                    AppendMessage(conversationBox, "Chatbot", $"Your favourite topic is {userData.FavouriteTopic}. Here's another tip: " +
                        $"{SelectRandomResponse(responses[userData.FavouriteTopic])}");
                else
                    AppendMessage(conversationBox, "Chatbot", "I don't know your favorite topic yet. Ask about cybersecurity topics to let me know!");
            }
            else
            {
                AppendMessage(conversationBox, "Chatbot", "I'm not sure how to answer that. Try asking about cybersecurity topics.");
            }
            return userInput;
        }

        public static void ShowLog(RichTextBox box)//(Troelsen, A. and Japikse, P., 2022)
        {
            var logs = ActivityLog.ShowLog(10); // Shows last 10 logs

            if (logs.Count == 0)
            {
                AppendMessage(box, "Chatbot", "The activity log is empty.");
            }
            else
            {
                AppendMessage(box, "Chatbot", "Recent activity log:");
                foreach (var entry in logs)
                {
                    AppendMessage(box, "Chatbot", entry);
                }
            }
        }

        public static void StartAddTask(RichTextBox conversationBox)//(Troelsen, A. and Japikse, P., 2022)
        {
            isAddingTask = true;
            addTaskStep = 1;
            newTask = new AddTask();
            AppendMessage(conversationBox, "Chatbot", "Let's add a task! What is the title?");
        }

        public static void CheckReminders(RichTextBox conversationBox, List<AddTask> tasks)//(Troelsen, A. and Japikse, P., 2022)
        {
            var dueTasks = tasks
                .Where(t => t.Reminder.HasValue && t.Reminder.Value <= DateTime.Now && !t.IsComplete)//(MicrosoftLearn, 2025)
                .ToList();

            foreach (var task in dueTasks)
            {
                AppendMessage(conversationBox, "Chatbot", $"Reminder: '{task.Title}' is due!");
                task.Reminder = null; // Remove the reminder after notifying
            }
        }

        public static void ContinueAddTask(string userInput, RichTextBox conversationBox, List<AddTask> tasks)//(Troelsen, A. and Japikse, P., 2022)
        {
            if (!isAddingTask)
                return;

            switch (addTaskStep)
            {
                case 1:
                    newTask.Title = userInput.Trim();
                    addTaskStep++;
                    AppendMessage(conversationBox, "Chatbot", "Please provide a description for this task.");
                    break;

                case 2:
                    newTask.Description = userInput.Trim();
                    addTaskStep++;
                    AppendMessage(conversationBox, "Chatbot", "Would you like to add a reminder? Type a date/time (e.g., 2025-06-15 14:30) or 'no'.");
                    break;

                case 3:
                    if(userInput.Trim().ToLower() == "no")
            {
                        newTask.Reminder = null;
                        tasks.Add(newTask);
                        AppendMessage(conversationBox, "Chatbot", $"Task '{newTask.Title}' added without a reminder.");

                        ActivityLog.AddLog($"Added task '{newTask.Title}' without a reminder.");

                        ResetAddTask();
                    }
            else
                    {
                        if (DateTime.TryParse(userInput.Trim(), out DateTime reminderDate))
                        {
                            newTask.Reminder = reminderDate;
                            tasks.Add(newTask);
                            AppendMessage(conversationBox, "Chatbot", $"Task '{newTask.Title}' added with reminder set for {reminderDate:g}.");

                            ActivityLog.AddLog($"Added task '{newTask.Title}' with reminder for {reminderDate:g}.");

                            ResetAddTask();
                        }
                        else
                        {
                            AppendMessage(conversationBox, "Chatbot", "Invalid date/time format. Please type the reminder date/time again or type 'no' to skip.");
                        }
                    }
                    break;
            }
        }

        public static void HandleDeleteTask(string input, RichTextBox box, List<AddTask> tasks)//(Troelsen, A. and Japikse, P., 2022)
        {
            var parts = input.Split(' ');

            if (tasks.Count == 0)
            {
                AppendMessage(box, "Chatbot", "You have no tasks to delete.");
                return;
            }

            if (parts.Length == 3 && int.TryParse(parts[2], out int index))
            {
                if (index >= 1 && index <= tasks.Count)
                {
                    var removedTask = tasks[index - 1];
                    var taskTitle = tasks[index - 1].Title;
                    tasks.RemoveAt(index - 1);
                    AppendMessage(box, "Chatbot", $"Deleted task #{index}: {removedTask.Title}");
                    ActivityLog.AddLog($"Deleted task '{taskTitle}'.");
                }
                else
                {
                    AppendMessage(box, "Chatbot", "That task number doesn't exist.");
                }
            }
            else
            {
                AppendMessage(box, "Chatbot", "Please use: delete task [task number]. Example: delete task 2");
            }
        }

        public static void HandleCompleteTask(string input, RichTextBox box, List<AddTask> tasks)//(Troelsen, A. and Japikse, P., 2022)
        {
            var parts = input.Split(' ');
            if (parts.Length == 3 && int.TryParse(parts[2], out int index) && index >= 1 && index <= tasks.Count)
            {
                tasks[index - 1].IsComplete = true;
                AppendMessage(box, "Chatbot", $"Marked task #{index} as complete.");
                ActivityLog.AddLog($"Marked task '{tasks[index - 1].Title}' as complete.");
            }
            else
            {
                AppendMessage(box, "Chatbot", "Usage: complete task [task number]");
            }
        }

        public static void ViewTasks(RichTextBox box, List<AddTask> tasks)//(Troelsen, A. and Japikse, P., 2022)
        {
            if (tasks.Count == 0)
            {
                AppendMessage(box, "Chatbot", "You have no tasks.");
            }
            else
            {
                AppendMessage(box, "Chatbot", "Here are your tasks:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    AppendMessage(box, "Chatbot", $"{i + 1}. {tasks[i]}");
                }
            }
        }

        private static void ResetAddTask()//(Troelsen, A. and Japikse, P., 2022)
        {
            isAddingTask = false;
            addTaskStep = 0;
            newTask = null;
        }

        public static bool IsAddingTask()
        {
            return isAddingTask;
        }

        private static List<QuizQuestionSave> quizQuestions = new List<QuizQuestionSave>//(Troelsen, A. and Japikse, P., 2022)
        {
            new QuizQuestionSave
            {
                Question = "Which of the following is a strong password?",
                Options = new List<string> { "123456", "Password123", "T3$tp@ssW0rd!", "qwerty" },
                CorrectAnswer = "T3$tp@ssW0rd!"
            },
            new QuizQuestionSave
            {
                Question = "True or False: It's safe to click links in emails from unknown senders.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False"
            },
            new QuizQuestionSave
            {
                Question = "Which is an example of phishing?",
                Options = new List<string> {
                    "A text from your friend",
                    "An email asking for your password with a suspicious link",
                    "A calendar reminder",
                    "A news website"
                },
                CorrectAnswer = "An email asking for your password with a suspicious link"
            },
            new QuizQuestionSave
            {
                Question = "True or False: Safe browsing means always updating your browser.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True"
            },
            new QuizQuestionSave
            {
                Question = "Social engineering is:",
                Options = new List<string> {
                    "Hacking websites",
                    "Manipulating people to give up sensitive information",
                    "Writing secure code",
                    "Buying antivirus software"
                },
                CorrectAnswer = "Manipulating people to give up sensitive information"
            },
            new QuizQuestionSave
            {
                Question = "Which should you check before clicking a link?",
                Options = new List<string> {
                    "The URL looks correct",
                    "It has 'https'",
                    "It’s from someone you trust",
                    "All of the above"
                },
                CorrectAnswer = "All of the above"
            },
            new QuizQuestionSave
            {
                Question = "True or False: Using the same password for all accounts is safe.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False"
            },
            new QuizQuestionSave
            {
                Question = "A popup says you won a prize. What should you do?",
                Options = new List<string> {
                    "Click it quickly",
                    "Share your email",
                    "Close the popup immediately",
                    "Enter your credit card to claim"
                },
                CorrectAnswer = "Close the popup immediately"
            },
            new QuizQuestionSave
            {
                Question = "True or False: Hackers never use phone calls to scam people.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False"
            },
            new QuizQuestionSave
            {
                Question = "Which is the safest way to store passwords?",
                Options = new List<string> {
                    "In a notebook",
                    "In a text file on your desktop",
                    "Using a password manager",
                    "Relying on memory"
                },
                CorrectAnswer = "Using a password manager"
            },
        };

        public static void StartQuiz(RichTextBox box)//(Troelsen, A. and Japikse, P., 2022)
        {
            quizInProgress = true;
            currentQuestionIndex = 0;
            correctAnswers = 0;
            incorrectQuestions.Clear();
            ActivityLog.AddLog("Started a cybersecurity quiz.");
            AppendMessage(box, "Chatbot", "Starting Cybersecurity Quiz! Answer the questions by typing the correct option.");
            AskNextQuestion(box);
        }

        private static void AskNextQuestion(RichTextBox box)//(Troelsen, A. and Japikse, P., 2022)
        {
            if (currentQuestionIndex < quizQuestions.Count)
            {
                var q = quizQuestions[currentQuestionIndex];
                StringBuilder questionText = new StringBuilder();
                questionText.AppendLine($"Question {currentQuestionIndex + 1}: {q.Question}");

                for (int i = 0; i < q.Options.Count; i++)
                {
                    questionText.AppendLine($"{i + 1}. {q.Options[i]}");
                }

                AppendMessage(box, "Chatbot", questionText.ToString());
            }
            else
            {
                EndQuiz(box);
            }
        }

        public static void HandleQuizAnswer(string input, RichTextBox box)//(Troelsen, A. and Japikse, P., 2022)
        {
            if (!quizInProgress) return;

            var q = quizQuestions[currentQuestionIndex];
            bool validInput = int.TryParse(input, out int choice);

            if (validInput && choice >= 1 && choice <= q.Options.Count)
            {
                string selected = q.Options[choice - 1];

                if (selected.Equals(q.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    AppendMessage(box, "Chatbot", "✅ Correct!");
                    correctAnswers++;
                }
                else
                {
                    AppendMessage(box, "Chatbot", $"❌ Incorrect! The correct answer is: {q.CorrectAnswer}");
                    incorrectQuestions.Add($"Q{currentQuestionIndex + 1}: {q.Question} (Correct: {q.CorrectAnswer})");
                }

                currentQuestionIndex++;
                AskNextQuestion(box);
            }
            else
            {
                AppendMessage(box, "Chatbot", "Invalid input. Please type the number of your answer.");
            }
        }

        private static void EndQuiz(RichTextBox box)//(Troelsen, A. and Japikse, P., 2022)
        {
            quizInProgress = false;
            AppendMessage(box, "Chatbot", $"Quiz Completed! :) You got {correctAnswers} out of {quizQuestions.Count} correct.");

            if (incorrectQuestions.Count > 0)
            {
                AppendMessage(box, "Chatbot", "Here are the questions you got wrong:");
                foreach (var item in incorrectQuestions)
                {
                    AppendMessage(box, "Chatbot", item);
                }
            }
            else
            {
                AppendMessage(box, "Chatbot", "Perfect Score! Well done! XD");
            }
            ActivityLog.AddLog($"Completed a quiz: {correctAnswers}/{quizQuestions.Count} correct.");
        }

        public static bool IsQuizInProgress()
        {
            return quizInProgress;
        }

        private static string SelectRandomResponse(List<string> responses)
        {
            return responses[random.Next(responses.Count)];
        }

        private static void AppendMessage(RichTextBox conversationBox, string sender, string message)//(Troelsen, A. and Japikse, P., 2022)
        {
            conversationBox.AppendText($"{sender}: {message}\n");

            // Speech synthesis for chatbot responses
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(message);
        }
    }
}
/*MicrosoftLearn, 2025. DateTime.Now Property[online] Available at:
   < https://learn.microsoft.com/en-us/dotnet/api/system.datetime.now?view=net-9.0 >
    [Accessed 25 June 2025]
Troelsen, A. and Japikse, P., 2022.Pro C# 10 with .NET 6. 11th ed. New York: Apress Media, LLC.
*/