using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{
    public class Question
    {
        public string QuestionTitle { get; set; } = string.Empty;


        public string Topic { get; set; } = string.Empty;
        public IEnumerable<string> Options { get; set; } = new List<string>();
        public string Answer { get; set; } = string.Empty;


        public string Time { get; set; } = string.Empty;
    }
}
