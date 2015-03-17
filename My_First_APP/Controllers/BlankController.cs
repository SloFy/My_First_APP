using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using My_First_APP.Models;
namespace My_First_APP.Controllers
{
    public class BlankController : Controller
    {
        //
        // GET: /Blank/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Input()
        {
            setSession();
            GenerateNewView();
            return View();

        }
        public void setSession()
        {
            if (Session["Question_ID"] == null)
                Session["Question_ID"] = 0;
        }
        public void Next_question()
        {
            Session["Question_ID"] = (Convert.ToInt32(Session["Question_ID"]) + 1).ToString();
        }
        public void Prev_question()
        {
            Session["Question_ID"] = (Convert.ToInt32(Session["Question_ID"]) - 1).ToString();
        }
        public void GenerateNewView()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DELL-PC;Initial Catalog=FC_DB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"))
                try
                {
                    connection.Open();
                    SqlCommand select = new SqlCommand("select count(*) from question;", connection);
                    ViewBag.Question_cnt = select.ExecuteScalar();
                    select.CommandText = "select text from question where id=" + Session["Question_ID"] + ";";
                    ViewBag.Question = select.ExecuteScalar();
                    select.CommandText = "select * from answer where question_id=" + Session["Question_ID"];
                    DataSet ds = new DataSet();
                    //  ViewBag.Cl_ans = new List<Ans>();
                    List<Ans> answer_list = new List<Ans>();
                    //   ViewBag.Answers = new List<string>();
                    //    ViewBag.Answers_type = new List<string>();
                    SqlDataAdapter da = new SqlDataAdapter(select);
                    da.Fill(ds, "answer");
                    foreach (DataRow row in ds.Tables["answer"].Rows)
                    {

                        //    ViewBag.Answers.Add(row["text"].ToString());
                        //      ViewBag.Answers_type.Add(row["type"].ToString());
                        Ans new_answer = new Ans((Convert.ToInt32(Session["Question_ID"])), row["text"].ToString(), row["type"].ToString());
                        answer_list.Add(new_answer);
                    }
                    ViewBag.Answer_list = answer_list;
                }
                catch
                {
                }
        }
        [HttpPost]
        string Array(List<string> CB_TB)
        {
            string fin = "";
            for (int i = 0; i < CB_TB.Count; i++)
                fin += CB_TB[i] + " ; ";
            return fin;
        }
        [HttpPost]
        public ActionResult Press(string action)
        {

            if (action == "Next")
                Next_question();
            if (action == "Prev")
                Prev_question();
            //string a = ViewData["value"].ToString();
            GenerateNewView();
     //       string a = ViewData["value"].ToString();
            return View("~/Views/Blank/Input.cshtml");


        }
        public void f()
        {

        }

    }
}