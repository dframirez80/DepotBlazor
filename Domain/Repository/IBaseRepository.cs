using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainRepository.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        #region Elimina una entidad asincronicos
        /// <summary>
        /// Elimina una entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a eliminar</param>
        /// <returns>numero de entidades eliminadas</returns>
        Task<int> DeleteAsync(T entity);
        #endregion

        #region Obtiene todas las entidades asincronicos
        /// <summary>
        /// Obtiene todas las entidades de tabla  asincronicos
        /// </summary>
        /// <returns>Lista de entidades</returns>
        Task<IEnumerable<T>?> GetsAsync();
        #endregion

        #region Obtiene entidad asincronicos
        /// <summary>
        /// Obtiene entidad  asincronicos
        /// </summary>
        /// <returns>Lista de entidades</returns>
        Task<T?> GetAsync(int id);
        #endregion

        #region Inserta entidad asincronicos
        /// <summary>
        /// Inserta entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a insertar</param>
        /// <returns>entidad</returns>
        Task<T?> InsertAsync(T entity);
        #endregion

        #region Actualiza entidad asincronicos
        /// <summary>
        /// Actualiza entidad en tabla asincronicos
        /// </summary>
        /// <param name="entity">entidad a actualiza</param>
        /// <returns>entidades actualizadas</returns>
        Task<int> UpdateAsync(T entity);
        #endregion

        #region Actualizar lista de entidades asincronicos
        /// <summary>
        /// Actualizar lista de entidades en tabla asincronicos
        /// </summary>
        /// <param name="entities">entidades a insertar</param>
        /// <returns>entidades actualizadas</returns>
        Task<int> UpdateAllAsync(IEnumerable<T> entities);
        #endregion

        #region Obtiene lista de una tabla segun parametros asincronicos
        /// <summary>
        /// Obtiene lista de una tabla asincronicos
        /// <para>Ejemplos</para>
        /// <para>GetListAsync(filter: x => x.Name == "Carlos"); lista donde campo Name es Carlos</para>
        /// <para>GetListAsync(orderBy: y => y.OrderBy(s => s.Name)); Lista ordenada por campo Name</para>
        /// <para>GetListAsync(includes: "Company"); Lista que incluye objeto Company</para>
        /// <para>GetListAsync(x => x.Name == "Carlos",y => y.OrderBy(s => s.Name),"Company");</para>
        /// </summary>
        /// <param name="filter">expresion</param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns>Lista de entidades</returns>
        Task<IEnumerable<T>?> GetListAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = "");
        #endregion
    }
}
