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
        public int UserVMId { get; set; }
        virtual public UserVM UserVM { get; set; }
        
        public int? GraphicVMId { get; set; }

        virtual public GraphTraningVM GraphicVM { get; set; }
    }
}
