using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sportclub.ViewModels
{
    public class ImageVM
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public byte[] ImageData { get; set; }
    }
}