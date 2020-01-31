using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportclub.ViewModel
{
    public class CoachesVM
    {
        public int Id { get; set; }
        
        public int UserVMId { get; set; }
        public UserVM UserVM { get; set; }
        public StatusCoach Status { get; set; }
        
        public int SpecializationVMId { get; set; }

        virtual public SpecializationVM SpecializationVM { get; set; }
        public double TimeWork { get; set; }


        public enum StatusCoach
        {
            COACHE, HEAD_COACHE_HALL, TOP_COACHE
        }
    }
}
