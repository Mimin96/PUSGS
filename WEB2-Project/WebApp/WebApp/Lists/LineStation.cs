using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Lists
{
    public class LineStation
    {
        public string Number { get; set; }

        public int IDtypeOfLine { get; set; }

        public string TypeOfLine { get; set; }

        public List<Station> Stations { get; set; }
    }
}