using Microsoft.AspNetCore.Components;
using MauiAppMCQs.Models;

namespace MauiAppMCQs.Pages
{
    public class QuizCardBase : ComponentBase
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        protected int questionIndex = 0;
        protected int score = 0;

        protected override Task OnInitializedAsync()
        {
            LoadQuestions();

            return base.OnInitializedAsync();
        }

        protected void OptionSelected(string option)
        {
            if (option == Questions[questionIndex].Answer)
            {
                score++;
            }
            questionIndex++;
        }

        protected void RestartQuiz()
        {
            score = 0;
            questionIndex = 0;
        }

        private void LoadQuestions()
        {
            Question q1 = new Question
            {
                QuestionTitle = "What is the capital of France?",
                Options = new List<string>() { "Nantes", "Lyon", "Paris", "Bruxelles" },
                Answer = "Paris"
            };

            Question q2 = new Question
            {
                QuestionTitle = "What is the capital of England?",
                Options = new List<string>() { "London", "Manchester", "Glasgow", "Edinburgh" },
                Answer = "London"
            };

            Question q3 = new Question
            {
                QuestionTitle = "What is the capital of Japan?",
                Options = new List<string>() { "Tokyo", "Seoul", "Kyoto", "Jakarta" },
                Answer = "Tokyo"
            };

            Question q4 = new Question
            {
                QuestionTitle = "What is the capital of Bangladesh?",
                Options = new List<string>() { "Islamabad", "Delhi", "Dakka", "Kolkata" },
                Answer = "Dakka"
            };


            Question q5 = new Question
            {
                QuestionTitle = "What is the capital of Egypt?",
                Options = new List<string>() { "Riyadh", "Cairo", "Tel Aviv", "Dubai" },
                Answer = "Cairo"
            };



            Questions.AddRange(new List<Question> { q1, q2, q3, q4, q5 });
        }
    }
}
