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

        public int? CoacheVMId { get; set; }
        public CoachesVM CoacheVM { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        //--------------------------


        public List<ClientsVM> ClientsVM { get; set; }


        [Required]
        [RegularExpression(@"(^(([0,1][0-9])|(2[0-3])):[0-5][0-9]$)", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeBegin { get; set; }

        [Required]
        [RegularExpression(@"(^([0-1]\d|2[0-3])(:[0-5]\d){2}$) ", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeEnd { get; set; }

        public GraphTraningVM()
        {
            this.ClientsVM = new List<ClientsVM>();
        }
        public IEnumerator<ClientsVM> GetEnumerator()
        {
            for (int i = 0; i < ClientsVM.Count; i++)
                yield return ClientsVM[i];
        }
    }
}
