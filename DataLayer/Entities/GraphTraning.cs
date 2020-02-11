using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Entities
{
    public class GraphTraning
    {
        public int Id { get; set; }

        [ForeignKey("Coache")]
        public int? CoacheId { get; set; }
        public Coaches Coache { get; set; }

        public int? GymsId { get; set; }
        public Gyms Gyms { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        //--------------------------
      
        //public List<Clients> Clients { get; set; } 
        public ICollection<Clients> Clients { get; set; }
        
        
        [Required]
        public DateTime TimeBegin { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }

        public GraphTraning()
        {
            this.Clients = new List<Clients>();
        }
        public IEnumerator<Clients> GetEnumerator()
        {
            for (int i = 0; i < Clients.Count; i++)
                yield return Clients.ToList()[i];
        }
    }
}