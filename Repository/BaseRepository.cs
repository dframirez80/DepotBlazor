using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DomainRepository.Repository;
using Repository.EntityDbContext;
using Microsoft.EntityFrameworkCore.Internal;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        #region Constructor y Context Property
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        #endregion

        // Metodos asincronicos
        #region Elimina una entidad asincronicos
        /// <summary>
        /// Elimina una entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a eliminar</param>
        /// <returns>numero de entidades eliminadas</returns>
        public async Task<int> DeleteAsync(T entity)
        {
            var _entity = _context.Set<T>();
            _entity.Remove(entity);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Obtiene todas las entidades asincronicos
        /// <summary>
        /// Obtiene todas las entidades de tabla  asincronicos
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public async Task<IEnumerable<T>?> GetsAsync()
        {
            var _entity = _context.Set<T>();
            return await _entity.ToListAsync();
        }
        #endregion

        #region Obtiene entidad asincronicos
        /// <summary>
        /// Obtiene entidad  asincronicos
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public async Task<T?> GetAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }
        #endregion

        #region Inserta entidad asincronicos
        /// <summary>
        /// Inserta entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a insertar</param>
        /// <returns>entidad</returns>
        public async Task<T?> InsertAsync(T entity)
        {
            var _entity = _context.Set<T>();
            await _entity.AddAsync(entity);
            int result = await _context.SaveChangesAsync();
            if (result == 0)
                return null;
            return entity;
        }
        #endregion

        #region Actualiza entidad asincronicos
        /// <summary>
        /// Actualiza entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a actualiza</param>
        /// <returns>entidades actualizadas</returns>
        public async Task<int> UpdateAsync(T entity)
        {
            var _entity = _context.Set<T>().AsNoTracking();
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Actualizar lista de entidades asincronicos
        /// <summary>
        /// Actualizar lista de entidades en tabla asincronicos
        /// </summary>
        /// <param name="entities">entidades a insertar</param>
        /// <returns>entidades actualizadas</returns>
        public async Task<int> UpdateAllAsync(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                var _entity = _context.Set<T>().AsNoTracking();
                //_entity.Attach(entity);
                _context.Entry(item).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Obtiene lista de una tabla segun parametros asincronicos
        /// <summary>
        /// Obtiene lista de una tabla asincronicos
        /// <para>Ejemplos</para>
        /// <para>ListTable(filter: x => x.Name == "Carlos"); lista donde campo Name es Carlos</para>
        /// <para>ListTable(orderBy: y => y.OrderBy(s => s.Name)); Lista ordenada por campo Name</para>
        /// <para>ListTable(includes: "Company"); Lista que incluye objeto Company</para>
        /// <para>ListTable(x => x.Name == "Carlos",y => y.OrderBy(s => s.Name),"Company");</para>
        /// </summary>
        /// <param name="filter">expresion</param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns>Lista de entidades</returns>
        public async Task<IEnumerable<T>?> GetListAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = "")
        {
            var _entity = _context.Set<T>().AsNoTracking();
            IQueryable<T> query = _entity;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }
        #endregion
    }
}
