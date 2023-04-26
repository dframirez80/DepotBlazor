using Domain.Entities;
using Domain.Models;

namespace Domain.Services
{
    public interface IProductService
    {
        #region Obtiene lista
        /// <summary>
        /// Obtiene lista
        /// </summary>
        /// <returns>Objeto con lista de entidades</returns>
        /// <remarks>
        /// Retorna 
        /// Response.Code = 0 Ok, < 0 Error .
        /// Response.Message = mensaje de error o cantidad de elementos de Response.Data;
        /// Response.Data lista de entidades;
        /// </remarks>
        Task<Response<List<ProductDto>>> GetAllAsync();
        #endregion

        #region Crea entidad
        /// <summary>
        /// Crea entidad
        /// </summary>
        /// <param name="entityDto">Objeto</param>
        /// <returns>Objeto con Entidad creada</returns>
        /// <remarks>
        /// Retorna 
        /// Response.Code = 0 Ok, < 0 Error .
        /// Response.Message = mensaje de error o cantidad de elementos de Response.Data;
        /// Response.Data Entidad creada;
        /// </remarks>
        Task<Response<ProductDto>> CreateAsync(ProductDto entityDto);
        #endregion

        #region Actualiza entidad
        /// <summary>
        /// Actualiza entidad
        /// </summary>
        /// <param name="entityDto">Objeto</param>
        /// <returns>Objeto con entidad actualizada</returns>
        /// <remarks>
        /// Retorna 
        /// Response.Code = 0 Ok, < 0 Error .
        /// Response.Message = mensaje de error o cantidad de elementos de Response.Data;
        /// Response.Data Entidad actualizada;
        /// </remarks>
        Task<Response<ProductDto>> UpdateAsync(ProductDto entityDto);
        #endregion

        #region Elimina entidad
        /// <summary>
        /// Elimina entidad
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Objeto</returns>
        /// <remarks>
        /// Retorna 
        /// Response.Code = 0 Ok, < 0 Error .
        /// Response.Message = mensaje de error o cantidad de elementos de Response.Data;
        /// Response.Data = false si no pudo eliminar entidad;
        /// </remarks>
        Task<Response<bool>> DeleteAsync(int id);
        #endregion
    }
}
