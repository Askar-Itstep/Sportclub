using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportclub.ViewModel
{
    public class GraphTraningVM
    {
        public int Id { get; set; }

        public int? CoacheId { get; set; }
        public CoachesVM Coache { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        //--------------------------


        public List<ClientsVM> Clients { get; set; }


        [Required]
        [RegularExpression(@"(^(([0,1][0-9])|(2[0-3])):[0-5][0-9]$)", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeBegin { get; set; }

        [Required]
        [RegularExpression(@"(^([0-1]\d|2[0-3])(:[0-5]\d){2}$) ", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeEnd { get; set; }

        public GraphTraningVM()
        {
            this.Clients = new List<ClientsVM>();
        }
        public IEnumerator<ClientsVM> GetEnumerator()
        {
            for (int i = 0; i < Clients.Count; i++)
                yield return Clients[i];
        }
    }
}
