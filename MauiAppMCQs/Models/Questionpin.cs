using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{
    public class Questionpin
    {
        [PrimaryKey, Indexed]


        public int ID { get; set; }
        public int pin { get; set; }

        

        internal IEnumerable<Question> Select(Func<object, Question> value)
        {
            throw new NotImplementedException();
        }
    }
}
