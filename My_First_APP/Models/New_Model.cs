using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class New_Model
    {
        public int blank_id;
        public int current_question=0;
        public Characterisitcs Question { get; set; }
        public List<Characterisitcs> questions { get; set; }
        public List<CharacteristicsValue> Default_Answers { get; set; }
        public List<string> s_answers;
        public List<string> type_answers;
        public List<bool> b_answers;
        public New_Model(New_Model m)
        {
            Default_Answers = new List<CharacteristicsValue>(m.Default_Answers);
            questions = new List<Characterisitcs>(m.questions);
            s_answers = new List<string>(m.s_answers);
            b_answers = new List<bool>(m.b_answers);
            type_answers = new List<string>(m.type_answers);
            blank_id = m.blank_id;
            current_question = m.current_question;         

        }
         public New_Model()
        {
           // String_User_Answers = new List<string>();
            Default_Answers = new List<CharacteristicsValue>();
            questions = new List<Characterisitcs>();
            s_answers = new List<string>();
            b_answers = new List<bool>();
            type_answers = new List<string>();
        }
    }
}