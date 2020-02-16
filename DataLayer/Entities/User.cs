using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }
        public string Phone { get; set; }
        
        public string Email { get; set; }
        public Gender Gender { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }

        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

       
        public string Token { get; set; }   //manager, coache

        [ForeignKey("Image")]
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}