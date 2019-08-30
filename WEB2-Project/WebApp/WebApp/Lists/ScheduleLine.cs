using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Lists
{
    public class ScheduleLine
    {
        public string Number { get; set; }
        public DateTime Time { get; set; }
        public string Day { get; set; }

        public int IDDay { get; set; }
    }
}