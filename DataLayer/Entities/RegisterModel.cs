using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLayer.Entities
{
    public class RegisterModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        
//^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$

        [RegularExpression(@"^(\s*)?(\+)?([-_():= +]?\d[-_():= +]?){10,14}(\s*)?$", ErrorMessage = "Некорректный номер телефона")]
        public string Phone { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный e-mail")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }

    }
}