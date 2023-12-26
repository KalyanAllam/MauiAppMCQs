﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{

    public class InQuestion


    {
        public int SNo { get; set; }
        public int  No { get; set; }

        public string Topic { get; set; }

        public string QuestionTitle { get; set; }

        public string Opt1 { get; set; }

        public string Opt2 { get; set; }

        public string Opt3 { get; set; }

        public string Opt4 { get; set; }

        public string Answer { get; set; }

        public int Time { get; set; }


        public int Correct { get; set; }

        public string Solution { get; set; }

    }
}
