using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Entities
{
    public class Administration
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public StatusManager Status { get; set; }
  


        public enum StatusManager
        {
           ADMIN,  MANAGER, TOP_MANAGER
        }
    }
}