 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sportclub.Entities
{
    public class Coaches
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public StatusCoach Status { get; set; }

        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }

        virtual public Specialization Specialization { get; set; }
        public double TimeWork { get; set; }  
      

        public enum StatusCoach
        {
            COACHE, HEAD_COACHE_HALL, TOP_COACHE
        }
       
    }
}