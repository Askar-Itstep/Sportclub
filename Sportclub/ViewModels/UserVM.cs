﻿using Sportclub.ViewModels;
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
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }
        
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\d{3}[\- ]?)?[\d ]{7,11}$", ErrorMessage = "Некорректный номер")]
        [Required]
        public string Phone { get; set; }


        [RegularExpression(@"\w[^@]+@([^@\.]+\.)+[^@\.]+\w", ErrorMessage = "Некорректный e-mail")]
        [Required]
        public string Email { get; set; }
        public GenderVM Gender { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }


        public int RoleId { get; set; }
        public RoleVM Role { get; set; }

        public string Token { get; set; }   //manager, coache

        public int ImageId { get; set; }
        public virtual ImageVM Image { get; set; }
    }
}

