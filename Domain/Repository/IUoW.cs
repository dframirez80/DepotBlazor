using Domain.Entities;
using DomainRepository.Repository;

namespace Domain.Repository
{
    public interface IUoW
    {
        IBaseRepository<User> Users { get; }
        IBaseRepository<Product> Products { get; }
        IBaseRepository<ProductMovement> ProductMovements { get; }
        IBaseRepository<Depot> Depots { get; }
    }
}
