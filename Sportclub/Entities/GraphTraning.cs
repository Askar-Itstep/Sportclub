using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sportclub.Entities
{
    public class GraphTraning
    {
        public int Id { get; set; }

        [ForeignKey("Coache")]
        public int? CoacheId { get; set; }
        public Coaches Coache { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        //--------------------------

      
        public List<Clients> Clients { get; set; } 

        #region int->TimeSpan
        //a)
        //public int HourBeginTick { get; set; }
        //public int MinuteBeginTick { get; set; }
        //public int HourEndTick { get; set; }
        //public int MinuteEndTick { get; set; }

        //[NotMapped]
        //public TimeSpan TimeBegin
        //{
        //    get { return new TimeSpan(HourBeginTick, MinuteBeginTick, 0); }
        //    set {
        //        HourBeginTick = value.Hours;
        //        MinuteBeginTick = value.Minutes;
        //    }
        //}
        #endregion
        
        [Required]
        [RegularExpression(@"(^(([0,1][0-9])|(2[0-3])):[0-5][0-9]$)", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeBegin { get; set; }

        [Required]
        [RegularExpression(@"(^([0-1]\d|2[0-3])(:[0-5]\d){2}$) ", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeEnd { get; set; }

        public GraphTraning()
        {
            this.Clients = new List<Clients>();
        }
        public IEnumerator<Clients> GetEnumerator()
        {
            for (int i = 0; i < Clients.Count; i++)
                yield return Clients[i];
        }
    }
}