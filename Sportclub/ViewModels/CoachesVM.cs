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
        
        public int UserId { get; set; }
        public UserVM User { get; set; }
        public StatusCoach Status { get; set; }
        
        public int SpecializationId { get; set; }

        virtual public SpecializationVM Specialization { get; set; }
        public double TimeWork { get; set; }


        public enum StatusCoach
        {
            COACHE, HEAD_COACHE_HALL, TOP_COACHE
        }
    }
}
