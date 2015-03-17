using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class MyModel
    {
       public int Num1, Num2, Result;
       public MyModel(int a, int b,int c)
       { Num1 = a; Num2 = b;Result=c; }
    }
    public class Ans
    {     
      public  int Q_ID;
      public  string answer;
      public  string type;
        
      public  Ans(int new_id,string new_answer,string new_type)
        {
            answer = new_answer;
            type = new_type;
            Q_ID = new_id;
        }
      public string get_answer()
      { return answer; }
      public int get_Q_ID()
      { return Q_ID; }
      public string get_type()
      { return type; }
    }
}