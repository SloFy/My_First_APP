using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace My_First_APP.Models
{
    public class ChangePasswordModel
    {
        

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [MinLength(6, ErrorMessage = "минимальная длина пароля- 6 символов")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "минимальная длина пароля- 6 символов")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]        
        public string ConfirmNewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "минимальная длина пароля- 6 символов")]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }
    }
    public class ChangeMailModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Новая почта")]
        [EmailAddress(ErrorMessage = "Неправильный адрес")]
        public string NewMail { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "минимальная длина пароля- 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
    public class ManagePerson
    {
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Пол")]
        public bool Sex { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

    }
}