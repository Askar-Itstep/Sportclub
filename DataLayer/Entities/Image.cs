﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Filename { get; set; }

        //public byte[] ImageData { get; set; }
        public string URI { get; set; }
    }
}