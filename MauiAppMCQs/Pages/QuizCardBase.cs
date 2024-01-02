using Microsoft.AspNetCore.Components;
using MauiAppMCQs.Models;
using Newtonsoft.Json;
using System.Timers;
using Microsoft.JSInterop;

using System.Text;
using System.Net.Http.Headers;

namespace MauiAppMCQs.Pages
{
    public class QuizCardBase : ComponentBase
    {
        public string code;
        public List<Question> Questions { get; set; } = new List<Question>();
        protected int questionIndex = 0;
        protected int score = 0;
        protected int failedIndex = 0;
        protected string[] failedQuestions = new string[500];
        public int totaltime; public int totalquestions;
       public int _currentCount=0;  
        private System.Timers.Timer _timer;
        [Inject]
        protected IJSRuntime JS { get; set; }  //used to call javascript from .NET method.
        private string selectedanswer="";         //used to store the selected answer.
        QuestionsDatabase questionsDatabase;
            public bool IsDisabled = true; 

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
            LoadQuestionsAsync();

            return base.OnInitializedAsync();
        }


        //    protected void OptionSelected(string option)
        protected async void AnswerSubmit()
        {
            IsDisabled=true ; 
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
                    failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle + "  Answer:  "  + Questions[questionIndex].Answer + "  Solution:  " + Questions[questionIndex].Solution;
                    failedIndex++;


                }
            }
            else
            {
                failedQuestions[failedIndex] = Questions[questionIndex].QuestionTitle + "  Answer:  " + Questions[questionIndex].Answer + "  Solution:  " + Questions[questionIndex].Solution;
                failedIndex++;
            }
            questionIndex++;
        }

        protected void OptionSelected(string option)
        {
                       IsDisabled = false; 
  ;
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
        }
        async Task<List<InQuestion>> GetApiData()
        {
        //    string apiUrl = "https://sheet2api.com/v1/UHC796KdSvqC/testsp";
       

            string jwturl = "https://quizapijwt.azurewebsites.net/api/";
            List<InQuestion> Questions = new List<InQuestion>();
             

            using (var client1 = new HttpClient())
            {
                try
                {
                    
                    var postData = new PostData
                    {
                        Name = "",

                        Password = ""
                    };
                    var jsonpost = System.Text.Json.JsonSerializer.Serialize(postData);
                    var content = new StringContent(jsonpost, Encoding.UTF8, "application/json");
                    client1.BaseAddress = new Uri(jwturl);

                    client1.DefaultRequestHeaders.Clear();
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client1.PostAsync("Auth", content);

                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        string json = await Res.Content.ReadAsStringAsync();

                        // Parse the JSON response to extract the address
                        dynamic data1 = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        code = data1.token;



                    }
                }
                catch (Exception ex)
                {
                     

                }

            }











            string connected  = "N";



            string apiUrl = "https://quizapijwt.azurewebsites.net/api/";
       
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make a GET request to the API
                    //    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {code}");
                    HttpResponseMessage response = await client.GetAsync("Questions");
                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                       
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

                        int existsdb;
                        foreach (InQuestion item in itemInTheDB)
                        {
                            var result = Questions.First(c => c.SNo == item.SNo);

                            if (result == null)
                            {
                                await questionsDatabase.DeleteItemAsync(item);
                            }
                        }

                        foreach (InQuestion item in  Questions)
                               {
                                  await questionsDatabase.SaveItemAsync(item);
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

            Questions.AddRange(res.Select(r => new Question
            {  SNo = r.No,
                Topic = r.Topic,
                QuestionTitle = r.QuestionTitle,
                Options = new List<string>() { r.Opt1, r.Opt2, r.Opt3, r.Opt4 },
                Answer = r.Answer,
                Time =  r.Time  ,
                Correct = r.Correct,
                Solution = r.Solution

            })); 

            totaltime = Questions.Sum(Question => Convert.ToInt32(Question.Time));

       totalquestions = (from e in res  select e.No).Count();  
        
            Questions= Questions.OrderByDescending(s => s.SNo).ToList();
        }




    }
       
}
