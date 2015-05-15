using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class FillingBlank
    {
        public List<Element> Elements;
        int User_ID;
        public FillingBlank()
        {
            Elements = new List<Element>();
        }
    }
}