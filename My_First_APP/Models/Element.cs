using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class Element
    {
        public Characterisitcs Question {get; set;}
        public List<CharacteristicsValue> Default_Answers { get; set; }
        public List<CurrentCharacteristicsValue> User_Answers { get; set; }
        //public List<string> String_User_Answers { get; set; }
        public Element()
        {
           // String_User_Answers = new List<string>();
            Default_Answers = new List<CharacteristicsValue>();
            User_Answers = new List<CurrentCharacteristicsValue>();            
        }
    }
}