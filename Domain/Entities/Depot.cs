﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Depot
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
