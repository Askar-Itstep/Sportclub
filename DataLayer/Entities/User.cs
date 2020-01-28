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
        //перенесено в register-model
        ///^(\s*)?(\+)?([-_():= +]?\d[-_():= +]?){10,14}(\s*)?$/
        ///^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$
        //[RegularExpression(@"(^\+\d{1,2})?((\(\d{3}\))|(\-?\d{3}\-)|(\d{3}))((\d{3}\-\d{4})|(\d{3}\-\d\d\  
        //            -\d\d)|(\d{7})|(\d{3}\-\d\-\d{3})) ", ErrorMessage = "Некорректный номер")]
        public string Phone { get; set; }
        

        //[RegularExpression(@"(/\A[^@]+@([^@\.]+\.)+[^@\.]+\z/)) ", ErrorMessage = "Некорректный e-mail")]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }

        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        ////2
        //public ISet<RoleName> Roles { get; set; }//не нужно-роли с поглощением
        //public enum RoleName //как добавить новую роль -> сделать табл.
        //{
        //    ADMIN, TOP, MANAGER, HEAD_COACHE, COACHE, CLIENT
        //}

        public string Token { get; set; }   //manager, coache
    }
}