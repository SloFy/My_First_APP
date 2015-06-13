using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using My_First_APP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Security.Principal;
namespace My_First_APP.Models
{
    public class Test
    {
        private FITNESSEntities db = new FITNESSEntities();
        public int blank_id;
        public int current_question;
        public int question_count;
        public List<string> s_answers;        
        public List<bool> b_answers;
        public Test(int blank_id,int current_question=0)
        {
            this.blank_id = blank_id;
            this.current_question = current_question;
            this.question_count = get_question_count();
            this.b_answers = new List<bool>(question_count);
            this.s_answers = new List<string>(question_count);
            for (int i = 0; i < question_count; i++)
            {
                this.b_answers.Add(false);
                this.s_answers.Add("");
            }
        }
        public Test(int blank_id,int current_question,  List<string> s_answers, List<bool> b_answers )
        {
            this.blank_id = blank_id;
            this.current_question = current_question;
            this.question_count = get_question_count();
            this.b_answers = b_answers;
            this.s_answers = s_answers;
        }
        public Test Create_New_Blank(int? blank_type,string login)
        {
            Blank blank=new Blank();
            blank.BlankType = db.BlankType.Where(b => b.Id == blank_type).FirstOrDefault();                   
            blank.Person = db.Person.Where(p => p.Account == (db.Account.Where(a => a.Login == login ).FirstOrDefault())).FirstOrDefault();
            blank.CreateDate = DateTime.Now.ToString();
            blank.Id = db.Blank.Max(m => m.Id)+1;
            db.Blank.Add(blank);
            db.SaveChanges();
            Test new_test = new Test(blank.Id);
            return new_test;
        }
        public Test()
        {
            this.blank_id = 0;
            this.current_question = 0;
            this.question_count = 0;
            this.b_answers = new List<bool>();
            this.s_answers = new List<string>();
        }
        
        public int get_question_count()
        {
           // return db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + db.Blank.Find(blank_id).BlankType.Id + ") ORDER BY Id").Count();
        return db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + db.Blank.Find(blank_id).BlankType.Id + ") ORDER BY Id").Count();
        }
        public Characterisitcs get_Characterisitcs()
        {
            return db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + db.Blank.Find(blank_id).BlankType.Id + ") ORDER BY Id").ElementAt(current_question);
        }
        public List<CharacteristicsValue> get_CharacteristicsValue()
        {
            int id = get_Characterisitcs().Id;
            return db.CharacteristicsValue.Where(c => c.CharID == id).ToList();
        }
        public void add_to_db()
        {
            CurrentCharacteristicsValue curr_char_value = new CurrentCharacteristicsValue();
            curr_char_value.BlankId = blank_id;
            curr_char_value.CharId = get_Characterisitcs().Id;
            int question_count = get_CharacteristicsValue().Count();
            for(int i=0;i<question_count;i++)
            {
                if((get_CharacteristicsValue()[i].Type=="CB")&&(b_answers[i]==true))
                {
                    curr_char_value.Value = get_CharacteristicsValue()[i].Value;
                    curr_char_value.Id = db.CurrentCharacteristicsValue.Max(c => c.Id) + 1;
                    db.CurrentCharacteristicsValue.Add(curr_char_value);
                    db.SaveChanges();
                }
                else if ((get_CharacteristicsValue()[i].Type=="TB"))
                {
                    curr_char_value.Value = s_answers[i];
                    curr_char_value.Id = db.CurrentCharacteristicsValue.Max(c => c.Id) + 1;
                    db.CurrentCharacteristicsValue.Add(curr_char_value);
                    db.SaveChanges();
                }
                else if ((get_CharacteristicsValue()[i].Type == "CB_TB") && (b_answers[i] == true))
                {
                    curr_char_value.Value = get_CharacteristicsValue()[i].Value;
                    curr_char_value.Id = db.CurrentCharacteristicsValue.Max(c => c.Id) + 1;
                    db.CurrentCharacteristicsValue.Add(curr_char_value);
                    db.SaveChanges();
                    CurrentCharacteristicsValue one_more_char_value = new CurrentCharacteristicsValue();
                    one_more_char_value.Value = s_answers[i];
                    one_more_char_value.BlankId = curr_char_value.BlankId;
                    one_more_char_value.CharId = curr_char_value.CharId;
                    one_more_char_value.Id = db.CurrentCharacteristicsValue.Max(c => c.Id) + 1;
                    db.CurrentCharacteristicsValue.Add(one_more_char_value);
                    db.SaveChanges();
                }
            }
        }
        public Test get_next(Test old)
        {
            Test next = new Test(old.blank_id,old.current_question+1);
            next.blank_id = old.blank_id;
            next.current_question = old.current_question+1;
            return next;
        }
        public bool Last()
        {
            return (current_question == question_count-1);
        }
        private FITNESSEntities get_db()
        {
            return  (new FITNESSEntities());
        }
    }
}