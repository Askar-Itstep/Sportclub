﻿using System.ComponentModel.DataAnnotations;

namespace Sportclub.Entities
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}