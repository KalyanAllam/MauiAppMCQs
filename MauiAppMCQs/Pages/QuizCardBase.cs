using Microsoft.AspNetCore.Components;
using MauiAppMCQs.Models;
using Newtonsoft.Json;
namespace MauiAppMCQs.Pages
{
    public class QuizCardBase : ComponentBase
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        protected int questionIndex = 0;
        protected int score = 0;
        protected int failedIndex = 0;
        protected string[] failedQuestions= new string[100];

        QuestionsDatabase questionsDatabase;
        

        protected override Task OnInitializedAsync()
        {
       //     questionsDatabase = new QuestionsDatabase();
            LoadQuestionsAsync();

            return base.OnInitializedAsync();
        }


        protected void OptionSelected(string option)
        {
            if (option.Trim() == Questions[questionIndex].Answer.Trim())
            {
                score++;
            }
            else
            {
                failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle + " " + Questions[questionIndex].Answer;
                failedIndex++;


            }
            questionIndex++;
        }
        protected void TakeQuiz()
        {
            score = 0;
            questionIndex = 0;
        }
        protected void RestartQuiz()
        {
            score = 0;
            questionIndex = 0;
            failedIndex = 0;
           failedQuestions[failedIndex] = "";
        }
        async Task<List<InQuestion>> GetApiData()
        {
            string apiUrl = "https://sheet2api.com/v1/wlS0h0USxm9p/quiz";
            List<InQuestion> Questions = new List<InQuestion>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make a GET request to the API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content of the response
                        string jsonString = await response.Content.ReadAsStringAsync();


                        // Deserialize the JSON string into an object
                        Questions = JsonConvert.DeserializeObject<List<InQuestion>>(jsonString);
                        // truncate sqlLIte table Load sqlite table  from  Questions
                        //If the first time when you load the api data, it will push data to the sqlite DB.
                    //      List<InQuestion> itemInTheDB = await questionsDatabase.GetItemsAsync();
                   //     if (itemInTheDB.Count == 0)
                   //     {
                   //         foreach (InQuestion item in  Questions)
                    //        {
                    //            await questionsDatabase.SaveItemAsync(item);
                    //        }
                   //     }  

                    }
                    else
                    { // Load  Questions from sql lite  table
                         Questions = await questionsDatabase.GetItemsAsync();

                    }
                }
                catch (Exception ex)
                {
                    Questions = await questionsDatabase.GetItemsAsync();
                }
            }
            return Questions;
        }

        private async Task LoadQuestionsAsync()
        {




            List<InQuestion> res = await GetApiData();

            Questions.AddRange(res.Select(r => new Question
            {
                QuestionTitle = r.QuestionTitle,
                Options = new List<string>() { r.Opt1, r.Opt2, r.Opt3, r.Opt4 },
                Answer = r.Answer
            }));

        }

    }
}
