using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class ProductMovement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;
        [Required]
        public int DepotIdSource { get; set; }

        [Required]
        public int DepotIdDestination { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
