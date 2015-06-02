using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data;
using System.Data.SqlClient;
using My_First_APP.Models;
namespace My_First_APP.Controllers
{
    public class BlankController : Controller
    {
        private FITNESSEntities db = new FITNESSEntities();
        //
        // GET: /Blank/

        public ActionResult Index()
        {
            // Account user = null;
             string login = User.Identity.GetUserName();            
            if (login == "" || login == null)
                return View("~/Views/Account/Registration.cshtml");
            
            int blank_cnt = db.Blank.Count(b => b.Person.Account.Login == login);
            if (blank_cnt == 0)
            {
                var BlankTypes = db.BlankType.Where(m => m.Id == 1);
                return View(BlankTypes);
            }
            else
            {
                var BlankTypes = db.BlankType.Where(m => m.Id != 1);
                return View(BlankTypes);
            }
        }
        /*
        public ActionResult Test(int Id=2)
        {
            string name = User.Identity.GetUserName();
            //Создание нового бланка
            Blank blank = new Blank();
            blank.BlankType = db.BlankType.Where(b => b.Id == Id).FirstOrDefault();                   
            blank.Person = db.Person.Where(p => p.Account == (db.Account.Where(a => a.Login == name ).FirstOrDefault())).FirstOrDefault();
            blank.CreateDate = DateTime.Now.ToString();
            int new_id = db.Blank.Max(m => m.Id)+1;
            blank.Id = new_id;
            db.Blank.Add(blank);
            db.SaveChanges();
            Session["Currnet_Blank"] = blank.Id;
            var Questions = db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + Id + ") ORDER BY Id").ToArray();
            
                      
            return View(Questions);
        }
         */
        public ActionResult Test(int Id=2)
        {
            string name = User.Identity.GetUserName();
            //Создание нового бланка
            Blank blank = new Blank();
            blank.BlankType = db.BlankType.Where(b => b.Id == Id).FirstOrDefault();                   
            blank.Person = db.Person.Where(p => p.Account == (db.Account.Where(a => a.Login == name ).FirstOrDefault())).FirstOrDefault();
            blank.CreateDate = DateTime.Now.ToString();
            int new_id = db.Blank.Max(m => m.Id)+1;
            blank.Id = new_id;
            db.Blank.Add(blank);
            db.SaveChanges();
            //Создание модели 
            New_Model mod = new New_Model();
            mod.blank_id = new_id;  
            mod.questions =db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + Id + ") ORDER BY Id").ToList();
            mod.Question = mod.questions.FirstOrDefault();
            mod.Default_Answers = db.CharacteristicsValue.Where(c => c.CharID == mod.Question.Id).ToList();
            mod.current_question = 0;
            for (int i = 0; i < mod.Default_Answers.Count; i++)
            {
                mod.b_answers.Add(false);
                mod.s_answers.Add("");
                mod.type_answers.Add(mod.Default_Answers[i].Type);
            }
            return View(mod);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Enter(int blank_id, int current_question, List<string> type_answers, List<bool> b_answers, List<string> s_answers)
        
        {
            /*
            Part(int blank_id, int current_question, List<string> type_answers, List<bool> b_answers, List<string> s_answers)
           ([Bind(Include = "blank_id,current_question,type_answers,b_answers,s_answers")] New_Model new_model)
            */
            New_Model new_model = new New_Model();
               new_model.blank_id = blank_id;
               new_model.current_question = current_question;
               new_model.type_answers = type_answers;  
               new_model.b_answers = b_answers;           
               new_model.s_answers = s_answers;
               new_model.questions = db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + db.Blank.Find(blank_id).BlankType.Id + ") ORDER BY Id").ToList();
               new_model.Question = new_model.questions[current_question];
               new_model.Default_Answers = db.CharacteristicsValue.Where(c => c.CharID == new_model.Question.Id).ToList();
            //Добавление ответов в БД
               new_model.AddAnswers();
            New_Model next = new New_Model();
            //Создание модели следующего вопроса.
            next.blank_id = new_model.blank_id;
            next.questions = new_model.questions;
            next.current_question = new_model.current_question + 1;
            next.Question = next.questions.ElementAt(next.current_question);
            next.Default_Answers=db.CharacteristicsValue.Where(c => c.CharID == next.Question.Id).ToList();
            for (int i = 0; i < next.Default_Answers.Count; i++)
            {
                next.b_answers.Add(false);
                next.s_answers.Add("");
                next.type_answers.Add(next.Default_Answers[i].Type);
            }
            new_model =null;
            return View("Test",next);
        }      
        
    }
}