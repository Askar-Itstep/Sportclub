using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sportclub.ViewModel
{
    public class AdministrationVM
    {
        public int Id { get; set; }

        public int UserVMId { get; set; }
        public UserVM UserVM { get; set; }

        [Required]
        public StatusManager Status { get; set; }

        public enum StatusManager
        {
            ADMIN, MANAGER, TOP_MANAGER
        }
    }
}