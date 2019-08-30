using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Lists
{
    public class PricelistLine
    {
        public DateTime ValidFrom { get; set; }
        public string TypeOfTicket { get; set; }
        public double Value { get; set; }
        public int IDPrice { get; set; }
        public int IDPriceList { get; set; }
    }
}