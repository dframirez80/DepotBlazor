using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;

namespace Domain.Services
{
    public class ProductService : IProductService
    {
        #region Variables y constructor
        private readonly IUoW _uow;
        private readonly IMapper _mapper;
        public ProductService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        #endregion

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
        public async Task<Response<List<ProductDto>>> GetAllAsync()
        {
            var response = new Response<List<ProductDto>>();
            try
            {
                var list = await _uow.Products.GetsAsync();
                if (list.Count() == 0 || list == null)
                {
                    response.Code = ErrorMessage.IERR_WITHOUT_REGISTER;
                    response.Message = ErrorMessage.SERR_WITHOUT_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<List<ProductDto>>(list);
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorMessage.IERR_EXCEPTION;
                response.Message = $"{ErrorMessage.SERR_EXCEPTION} {ex.Message}";
                return response;
            }
        }
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
        public async Task<Response<ProductDto>> CreateAsync(ProductDto entityDto)
        {
            var response = new Response<ProductDto>();
            if (entityDto == null || entityDto.DepotId <= 0 ||
                string.IsNullOrEmpty(entityDto.Description) || 
                string.IsNullOrEmpty(entityDto.Code) || 
                string.IsNullOrEmpty(entityDto.DepotName))
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.Products.GetListAsync(filter: x => x.Code == entityDto.Code && x.DepotId == entityDto.DepotId);
                if (exists != null && exists.Count() > 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_EXIST;
                    response.Message = ErrorMessage.SERR_IDENTITY_EXIST;
                    return response;
                }
                var entity = _mapper.Map<Product>(entityDto);
                entity.Created = DateTime.Now;
                entity.Modified = DateTime.Now;
                var result = await _uow.Products.InsertAsync(entity);
                if (result == null)
                {
                    response.Code = ErrorMessage.IERR_CREATE_REGISTER;
                    response.Message = ErrorMessage.SERR_CREATE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<ProductDto>(entity);
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorMessage.IERR_EXCEPTION;
                response.Message = $"{ErrorMessage.SERR_EXCEPTION} {ex.Message}";
                return response;
            }
        }
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
        public async Task<Response<ProductDto>> UpdateAsync(ProductDto entityDto)
        {
            var response = new Response<ProductDto>();
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Description) || string.IsNullOrEmpty(entityDto.Code))
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.Products.GetListAsync(filter:x => x.Id == entityDto.Id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                //actualiza si cambio descripcion o codigo
                var newCode = exists.First().Code;
                var newDescription = exists.First().Description;
                if (entityDto.Code != newCode || entityDto.Description != newDescription) {
                    // verifico si existe el codigo nuevo
                    if (entityDto.Code != newCode)
                    {
                        exists = await _uow.Products.GetListAsync(filter: x => x.Code == entityDto.Code);
                        if (exists.Count() > 0)
                        {
                            response.Code = ErrorMessage.IERR_IDENTITY_EXIST;
                            response.Message = ErrorMessage.SERR_IDENTITY_EXIST;
                            return response;
                        }
                    }
                    exists = await _uow.Products.GetListAsync(filter: x => x.Code == newCode);
                    foreach (var item in exists)
                    {
                        item.Modified = DateTime.Now;
                        item.Code = entityDto.Code;
                        item.Description = entityDto.Description;
                        // cambia solo por id
                        if (item.Id == entityDto.Id) 
                            item.Quantity = entityDto.Quantity;
                    }
                    int res = await _uow.Products.UpdateAllAsync(exists);
                    if (res == 0)
                    {
                        response.Code = ErrorMessage.IERR_UPDATE_REGISTER;
                        response.Message = ErrorMessage.SERR_UPDATE_REGISTER;
                        return response;
                    }
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = entityDto;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorMessage.IERR_EXCEPTION;
                response.Message = $"{ErrorMessage.SERR_EXCEPTION} {ex.Message}";
                return response;
            }
        }
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
        public async Task<Response<bool>> DeleteAsync(int id)
        {
            var response = new Response<bool>();
            if (id <= 0)
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.Products.GetListAsync(filter:x => x.Id == id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                int result = await _uow.Products.DeleteAsync(exists.First());
                if (result == 0)
                {
                    response.Code = ErrorMessage.IERR_DELETE_REGISTER;
                    response.Message = ErrorMessage.SERR_DELETE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorMessage.IERR_EXCEPTION;
                response.Message = $"{ErrorMessage.SERR_EXCEPTION} {ex.Message}";
                return response;
            }
        }
        #endregion
    }
}
