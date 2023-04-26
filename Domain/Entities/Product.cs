using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Code { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Quantity { get; set; } = 0;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public int DepotId { get; set; }
        public string DepotName { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
