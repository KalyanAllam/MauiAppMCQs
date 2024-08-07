﻿using Microsoft.AspNetCore.Components;
using MauiAppMCQs.Models;
using Newtonsoft.Json;
using System.Timers;
using Microsoft.JSInterop;


using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MauiAppMCQs.Pages
{
    public class QuizCardBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        public string code;public string jwttoken;
        public List<Question> Questions { get; set; } = new List<Question>();
        protected int questionIndex = 0;
        protected int score = 0;
        protected int failedIndex = 0;
        protected string[] failedQuestions = new string[500];
        protected string[] failedAnswer = new string[500];
        protected string[] failedSolution = new string[500];

        public int totaltime; public int totalquestions;
        public int _currentCount = 0;
        private System.Timers.Timer _timer;
        [Inject]
        protected IJSRuntime JS { get; set; }  //used to call javascript from .NET method.
        private string selectedanswer = "";         //used to store the selected answer.
        MyClass myclass;
        QuestionsDatabase questionsDatabase;
        public bool IsDisabled = true;
        public string matches;
        public string value = "";
        protected override Task OnInitializedAsync()
        {
            _timer = new();
            _timer.Interval = 1000;
            _timer.Elapsed += async (object? sender, ElapsedEventArgs e) =>
            {
                _currentCount++;
                await InvokeAsync(StateHasChanged);
            };
            _timer.Enabled = true;
            //  Uncomment below line if db needs to be loaded
            questionsDatabase = new QuestionsDatabase();
            myclass = new MyClass();
            LoadQuestionsAsync();

            return base.OnInitializedAsync();
        }


        //    protected void OptionSelected(string option)




        protected async void AnswerSkip()
        {
            IsDisabled = true;
            value = selectedanswer;
            await JS.InvokeVoidAsync("ClearStatus");  //call the javascript function to clear the radio button status when question changes.
            failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle;
            failedAnswer[failedIndex] = Questions[questionIndex].Answer;
            failedSolution[failedIndex] = Questions[questionIndex].Solution;
            failedIndex++;
            questionIndex++;
        }
        protected async void AnswerSubmit()
        {
            IsDisabled = true;
            value = selectedanswer;
            await JS.InvokeVoidAsync("ClearStatus");  //call the javascript function to clear the radio button status when question changes.

            if (value != null)
            {
                if ((Questions[questionIndex].Options.ToList().IndexOf(selectedanswer.Trim()) + 1) == Questions[questionIndex].Correct)
                {
                    score++;
                }
                else
                {
                    failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle  ;
                    failedAnswer[failedIndex] = Questions[questionIndex].Answer;
                    failedSolution[failedIndex]=Questions[questionIndex].Solution;
                    failedIndex++;


                }
            }
            else
            {
                failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle;
                failedAnswer[failedIndex] = Questions[questionIndex].Answer;
                failedSolution[failedIndex] = Questions[questionIndex].Solution;
                failedIndex++;
            }
            questionIndex++;
        }

        protected void OptionSelected(string option)
        {
            IsDisabled = false;
            
            selectedanswer = option.Trim();
        }

        protected void TakeQuiz()
        {
            _currentCount = 0;
            score = 0;
            questionIndex = 0;
        }
        protected void RestartQuiz()
        {
            _currentCount = 0;
            score = 0;
            questionIndex = 0;
            failedIndex = 0;
            failedQuestions[failedIndex] = "";
            failedAnswer[failedIndex] = "";
            failedSolution[failedIndex] = "";
        }
        async Task<List<InQuestion>> GetApiData()
        {
            //    string apiUrl = "https://sheet2api.com/v1/UHC796KdSvqC/testsp";


        string jwturl = "https://supaquizapi.azurewebsites.net/api/";

          //  string jwturl = "https://quizapijwt.azurewebsites.net/api/";


            List<InQuestion> Questions = new List<InQuestion>();




            var jsonContent = new StringContent(JsonConvert.SerializeObject(new
            {
                bodyField1 = ""

            }), Encoding.UTF8, "application/json");



            using (var client1 = new HttpClient())
            {
                string authurl = "https://myquizapi.azurewebsites.net/api/";

                client1.BaseAddress = new Uri(authurl);

                client1.DefaultRequestHeaders.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client1.PostAsync("Auth", jsonContent);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    string json = await Res.Content.ReadAsStringAsync();

                    // Parse the JSON response to extract the address
                    dynamic data1 = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    jwttoken = data1.token;




                }

            }









            string connected = "N";



              string apiUrl = "https://myquizapi.azurewebsites.net/api/";  
          
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make a GET request to the API
                    //    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    client.BaseAddress = new Uri(jwturl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwttoken}");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = null;
                    response = await client.GetAsync("Questions");
                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                      //  response = await client.GetAsync("Questions");
                        // Make a GET request to the API

                        connected = "Y";
                        // Read the content of the response
                        string jsonString = await response.Content.ReadAsStringAsync();


                        // Deserialize the JSON string into an object
                        Questions = JsonConvert.DeserializeObject<List<InQuestion>>(jsonString);
                        // truncate sqlLIte table Load sqlite table  from  Questions
                        //If the first time when you load the api data, it will push data to the sqlite DB.

                        //comment begins

                        List<InQuestion> itemInTheDB = await questionsDatabase.GetItemsAsync();
                        //    if (itemInTheDB.Count == 0)
                        // {


                        foreach (InQuestion item in itemInTheDB)
                        {
                            var result = Questions.FirstOrDefault(c => c.SNo == item.SNo);

                            if (result == null)
                            {
                                await questionsDatabase.DeleteItemAsync(item);
                            }
                        }

                        foreach (InQuestion itemdb in Questions)
                        {
                            var resulty = itemInTheDB.FirstOrDefault(c => c.SNo == itemdb.SNo);
                            matches = "N";


                            if (resulty == null)
                            { await questionsDatabase.SaveItemAsync(itemdb); }


                            if (resulty != null)
                            {
                                if ((resulty.No == itemdb.No) && (resulty.Opt1 == itemdb.Opt1) && (resulty.Opt2 == itemdb.Opt2) && (resulty.Opt3 == itemdb.Opt3) && (resulty.Opt4 == itemdb.Opt4)
                                 && (resulty.QuestionTitle == itemdb.QuestionTitle) && (resulty.Solution == itemdb.Solution) && (resulty.Time == itemdb.Time) && (resulty.Answer == itemdb.Answer)
                                 && (resulty.Topic == itemdb.Topic))
                                { matches = "Y"; }

                            }
                            else
                            { await questionsDatabase.SaveItemAsync(itemdb); }
                            if (matches == "N")
                            { await questionsDatabase.SaveItemAsync(itemdb); }

                        }
                        //   }
                        //comment ends

                    }
                    else
                    { // Load  Questions from sql lite  table
                        Questions = await questionsDatabase.GetItemsAsync();

                    }
                }
                catch (Exception ex)
                {
                    if (connected == "N")
                    {
                        Questions = await questionsDatabase.GetItemsAsync();

                    }


                }
            }
            return Questions;
        }

        private async Task LoadQuestionsAsync()
        {



            List<InQuestion> res = await GetApiData();
            List<InQuestion> dest;
               string stopflag = "Y";

            int maxnumber = (from e in res select e.No).Max();
            int minumber = (from e in res select e.No).Min();
            int range = maxnumber + 1;
            Random rand = new Random();
            int ranumber;

            int newsno = 0;
            while (stopflag == "Y")
            {
                newsno = newsno + 1;
                ranumber = rand.Next(0, range);
                var result = Questions.FirstOrDefault(c => c.SNo == ranumber);
            //   dest= res.FirstOrDefault(c => c.No == ranumber);
                if (result == null)
            {
                Questions.AddRange(res.Where(r => r.No == ranumber).Select(r => new Question
                {
                    SNo = r.No,
                    Topic = r.Topic,
                    QuestionTitle = r.QuestionTitle,
                    Options = new List<string>() { r.Opt1, r.Opt2, r.Opt3, r.Opt4 },
                    Answer = r.Answer,
                    Time = r.Time,
                    Correct = r.Correct,
                    Solution = r.Solution

                }));
                }
                int count = (from e in Questions select e.SNo).Count();
                 
                if (count == Id)
                { stopflag = "N"; }
            }

             

            totaltime = Questions.Sum(Question => Convert.ToInt32(Question.Time));
         int questionSno = 0;
            foreach (var question in Questions)
             {
               questionSno = questionSno + 1;
                question.SNo = questionSno;
           }




            totalquestions = (from e in Questions select e.SNo).Count();
            maxnumber = (from e in Questions select e.SNo).Max();
            minumber = (from e in Questions select e.SNo).Min();
        //    Questions = Questions.OrderByDescending(s => s.SNo).ToList();
        }




    }

}
