using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sportclub.Entities
{
    public class Administration
    {
        [Key]
        public int Id { get; set; }
        //public string FullName { get; set; }
        //public DateTime BirthDay { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public Gender Gender { get; set; }  
        //public string Login { get; set; }
        //public string Password { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        virtual public User User { get; set; }

        [Required]
        public StatusManager Status { get; set; }
  


        public enum StatusManager
        {
           ADMIN,  MANAGER, TOP_MANAGER
        }
    }
}