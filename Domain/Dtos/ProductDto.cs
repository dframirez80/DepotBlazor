using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        
        public int DepotId { get; set; }
        public string DepotName { get; set; }

        public int UserId { get; set; }
        public UserDto? User { get; set; }
    }
}
