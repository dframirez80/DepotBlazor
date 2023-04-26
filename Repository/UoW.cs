using Domain.Entities;
using Domain.Repository;
using DomainRepository.Repository;
using Repository.EntityDbContext;

namespace Repository.Repositories
{
    public class UoW : IUoW
    {
        #region Variables
        private readonly AppDbContext _context;
        public IBaseRepository<User> Users { get; }
        public IBaseRepository<Product> Products { get; }
        public IBaseRepository<ProductMovement> ProductMovements { get; }
        public IBaseRepository<Depot> Depots { get; }
        #endregion

        #region Constructor
        public UoW(AppDbContext dbContext)
        {
            _context = dbContext;
            Users = new BaseRepository<User>(_context);
            Products = new BaseRepository<Product>(_context);
            Depots = new BaseRepository<Depot>(_context);
            ProductMovements = new BaseRepository<ProductMovement>(_context);
        }
        #endregion
    }
}
