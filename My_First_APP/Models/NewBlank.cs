using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace My_First_APP.Models
{
    public class NewBlank
    {
        public NewBlank()
        {
            this.CharacteristicsValue = new HashSet<CharacteristicsValue>();
        }       
       public string[] s_values;
       public bool[] b_values;
       public int blank_id;
       public int charachter_id;
       public Characterisitcs Characteristics { get; set; }
       public ICollection<CharacteristicsValue> CharacteristicsValue { get; set; }
       public CurrentCharacteristicsValue CurrentCharacteristicsValue { get; set; }
    }
    
}