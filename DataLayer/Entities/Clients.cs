using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Entities
{
    public class Clients
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        virtual public User User { get; set; }

        [ForeignKey("Graphic")]
        public int? GraphicId { get; set; }
        
        virtual public GraphTraning Graphic { get; set; }
    }
}