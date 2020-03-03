using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportclub.ViewModel
{
    public class ClientsVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        virtual public UserVM User { get; set; }

        public ICollection<GraphTraningVM> GraphTraning { get; set; }
    }
}
