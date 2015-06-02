using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class InputModel
    {
        private FITNESSEntities db = new FITNESSEntities();
        public int blank_id;
        public int current_question;
        public Characterisitcs Question { get; set; }
        public List<Characterisitcs> questions { get; set; }
        public List<CharacteristicsValue> Default_Answers { get; set; }
        public List<string> s_answers;
        public List<string> type_answers;
        public List<bool> b_answers;
        public InputModel(InputModel m)
        {
            Default_Answers = new List<CharacteristicsValue>(m.Default_Answers);
            questions = new List<Characterisitcs>(m.questions);
            s_answers = new List<string>(m.s_answers);
            b_answers = new List<bool>(m.b_answers);
            type_answers = new List<string>(m.type_answers);
            blank_id = m.blank_id;
            current_question = m.current_question;
            Question = new Characterisitcs();
            Question = m.Question;
        }
        public InputModel(int new_blank_id,int new_current_question,List<string> new_s_answers,List<string> new_type_answers,List<bool> new_b_answers)
        {
            blank_id = new_blank_id;
            current_question = new_current_question;
            type_answers = new_type_answers;
            s_answers = new_s_answers;
            b_answers = new_b_answers;
        }
         public InputModel()
        {
           // String_User_Answers = new List<string>();
            Default_Answers = new List<CharacteristicsValue>();
            questions = new List<Characterisitcs>();
            s_answers = new List<string>();
            b_answers = new List<bool>();
            type_answers = new List<string>();
        }
        public void AddAnswers()
        {
             CurrentCharacteristicsValue curr_CHV = new CurrentCharacteristicsValue();
               curr_CHV.BlankId = blank_id;
               curr_CHV.CharId = Question.Id;
               
            for(int i=0;i<Default_Answers.Count();i++)
            {

                if ((Default_Answers[i].Type == "CB") && (b_answers[i] == true))
                {
                                    
                    curr_CHV.Value=Default_Answers[i].Value;
                    curr_CHV.Id = (db.CurrentCharacteristicsValue.Max(m => m.Id) + 1);
                    db.CurrentCharacteristicsValue.Add(curr_CHV);
                    db.SaveChanges();
                }
                if (Default_Answers[i].Type == "TB")
                {
                   
                    curr_CHV.Value = s_answers[i];
                    curr_CHV.Id = (db.CurrentCharacteristicsValue.Max(m => m.Id) + 1);
                    db.CurrentCharacteristicsValue.Add(curr_CHV);
                    db.SaveChanges();
                }
                if ((Default_Answers[i].Type == "CB_TB") && (b_answers[i] == true))
                {
                    curr_CHV.Value = Default_Answers[i].Value;
                    curr_CHV.Id = (db.CurrentCharacteristicsValue.Max(m => m.Id) + 1);
                    db.CurrentCharacteristicsValue.Add(curr_CHV);
                    db.SaveChanges();
                    curr_CHV.Value = s_answers[i];
                    curr_CHV.Id = (db.CurrentCharacteristicsValue.Max(m => m.Id) + 1);
                    db.CurrentCharacteristicsValue.Add(curr_CHV);
                    db.SaveChanges();
                }
            }
        }
    }
}