using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{
    public class QuestionF
    {
        [PrimaryKey, Indexed]


        public int SNo { get; set; }
        public int No { get; set; }

        public string Topic { get; set; }

        public string QuestionTitle { get; set; }

  

        public string Answer { get; set; }

        public byte[] Imagedata { get; set; }


        internal IEnumerable<Question> Select(Func<object, Question> value)
        {
            throw new NotImplementedException();
        }
    }
}
