using Sportclub.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportclub.ViewModel
{
    public class UserVM
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }


        //[RegularExpression(
        //    @"(^\+\d{1,2})?((\(\d{3}\))|(\-?\d{3}\-)|(\d{3}))((\d{3}\-\d{4})|(\d{3}\-\d\d\ -\d\d)
        //        |(\d{7})|(\d{3}\-\d\-\d{3}))", ErrorMessage = "Некорректный номер")]
        public string Phone { get; set; }


        //[RegularExpression(@"(/\A[^@]+@([^@\.]+\.)+[^@\.]+\z/) ", ErrorMessage = "Некорректный e-mail")]
        public string Email { get; set; }
        public GenderVM Gender { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }


        public int RoleId { get; set; }
        public RoleVM Role { get; set; }

        public string Token { get; set; }   //manager, coache

        public int ImageId { get; set; }
        public virtual ImageVM Image { get; set; }
    }
}

