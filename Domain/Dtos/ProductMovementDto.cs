using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductMovementDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int DepotIdSource { get; set; }
        public int DepotIdDestination { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
