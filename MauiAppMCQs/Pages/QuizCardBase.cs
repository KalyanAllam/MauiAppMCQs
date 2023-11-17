using Microsoft.AspNetCore.Components;
using MauiAppMCQs.Models;

namespace MauiAppMCQs.Pages
{
    public class QuizCardBase : ComponentBase
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        protected int questionIndex = 0;
        protected int score = 0;
        protected int failedIndex = 0;
        protected string[] failedQuestions= new string[100];
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
            else
            {
                failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle;
                failedIndex++;


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
                Options = new List<string>() { "Nantes", "Lyon", "Paris", "Bern" },
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

            Question q6 = new Question
            {
                QuestionTitle = "What is the capital of Mongolia?",
                Options = new List<string>() { "Nairobi", "Shanghai", "Hubei", "Ulan Bator" },
                Answer = "Ulan Bator"
            };
            Question q7 = new Question
            {
                QuestionTitle = "What is the capital of Iran?",
                Options = new List<string>() { "Riyadh", "Tehran", "Bagdhad", "Kabul" },
                Answer = "Tehran"
            };
            Question q8 = new Question
            {
                QuestionTitle = "What is the capital of Portugal?",
                Options = new List<string>() { "Lisbon", "Madrid", "Barcelona", "Milan" },
                Answer = "Lisbon"
            };
            Question q9 = new Question
            {
                QuestionTitle = "What is the capital of USA?",
                Options = new List<string>() { "London", "Washington DC", "Dublin", "Toronto" },
                Answer = "Washington DC"
            };
            Question q10 = new Question
            {
                QuestionTitle = "What is the capital of Argentina?",
                Options = new List<string>() { "Mexico City", "Bogota", "Buenos Aires", "Brasillia" },
                Answer = "Buenos Aires"
            };
            Question q11= new Question
            {
                QuestionTitle = "What is the capital of Ukraine?",
                Options = new List<string>() { "Kyiv", "Minsk", "Budapest", "Bucharest" },
                Answer = "Kyiv"
            };
            Question q12 = new Question
            {
                QuestionTitle = "What is the capital of Turkey?",
                Options = new List<string>() { "Damascus", "Athens", "Riyadh", "Ankara" },
                Answer = "Ankara"
            };
            Question q13 = new Question
            {
                QuestionTitle = "What is the capital of Kenya?",
                Options = new List<string>() { "Lusaka", "Kampala", "Nairobi", "Addis Ababa" },
                Answer = "Nairobi"
            };
            Question q14 = new Question
            {
                QuestionTitle = "What is the capital of Zaire?",
                Options = new List<string>() { "Lusaka", "Maputo", "Nairobi", "Kinshasa" },
                Answer = "Kinshasa"
            };
            Question q15= new Question
            {
                QuestionTitle = "What is the capital of Ghana?",
                Options = new List<string>() { "Accra", "Maputo", "Nairobi", "Kinshasa" },
                Answer = "Accra"
            };

            Question q16 = new Question
            {
                QuestionTitle = "What is the capital of New Zealand?",
                Options = new List<string>() { "Melbourne", "Auckland", "Wellington", "Sydney" },
                Answer = "Wellington"
            };
            Question q17 = new Question
            {
                QuestionTitle = "What is the capital of Lithuania?",
                Options = new List<string>() { "Minsk", "Leningrad", "Vilnuis", "Moscow" },
                Answer = "Vilnuis"
            };
            Question q18 = new Question
            {
                QuestionTitle = "What is the capital of Canada?",
                Options = new List<string>() { "Toronto", "London", "Ottawa", "Vancouver" },
                Answer = "Ottawa"
            };
            Question q19 = new Question
            {
                QuestionTitle = "What is the capital of India?",
                Options = new List<string>() { "New Delhi", "Ahmedabad", "Kolkata", "Mumbai" },
                Answer = "New Delhi"
            };

            Question q20 = new Question
            {
                QuestionTitle = "What is the capital of Afghanistan?",
                Options = new List<string>() { "Tashkent", "Bagdhad", "Kabul", "Sharjah" },
                Answer = "Kabul"
            };


            Question q21 = new Question
            {
                QuestionTitle = "What is the capital of Thailand?",
                Options = new List<string>() { "Jakarta", "Bangkok", "Hanoi", "Manila" },
                Answer = "Bangkok"
            };


            Question q22 = new Question
            {
                QuestionTitle = "What is the capital of Indonesia?",
                Options = new List<string>() { "Jakarta", "Bangkok", "Seoul", "Kualalampur" },
                Answer = "Jakarta"
            };

            Question q23 = new Question
            {
                QuestionTitle = "What is the capital of Peru?",
                Options = new List<string>() { "Santiago", "Lima", "Buenos Aries", "Quito" },
                Answer = "Lima"
            };

            Question q24 = new Question
            {
                QuestionTitle = "What is the capital of Costa Rica?",
                Options = new List<string>() { "Managua", "San Salvador", "San Jose", "Quito" },
                Answer = "San Jose"
            };

            Question q25 = new Question
            {
                QuestionTitle = "What is the capital of Fiji?",
                Options = new List<string>() { "Port Vila", "Suva", "Papeete", "Port Vila" },
                Answer = "Suva"
            };

            Questions.AddRange(new List<Question> { q1, q2, q3, q4, q5 ,q6,q7,q8,q9,q10,q11,q12,q13,q14,q15,q16,q17,q18,q19,q20,q21,q22,q23,q24,q25});
        }
    }
}
