using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;

namespace Domain.Services
{
    public class ProductMovementService : IProductMovementService
    {
        #region Variables y constructor
        private readonly IUoW _uow;
        private readonly IMapper _mapper;
        public ProductMovementService(IUoW uow, IMapper mapper)
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
        public async Task<Response<List<ProductMovementDto>>> GetAllAsync()
        {
            var response = new Response<List<ProductMovementDto>>();
            try
            {
                var list = await _uow.ProductMovements.GetsAsync();
                if (list.Count() == 0 || list == null)
                {
                    response.Code = ErrorMessage.IERR_WITHOUT_REGISTER;
                    response.Message = ErrorMessage.SERR_WITHOUT_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<List<ProductMovementDto>>(list);
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
        public async Task<Response<ProductMovementDto>> CreateAsync(ProductMovementDto entityDto)
        {
            var response = new Response<ProductMovementDto>();
            if (entityDto == null || entityDto.UserId < 1 || 
                entityDto.ProductId < 1 || entityDto.DepotIdSource < 1 || 
                entityDto.DepotIdDestination < 1 || entityDto.Quantity < 1)
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                // verifica si existe deposito origen
                var depotSource = await _uow.Depots.GetAsync(entityDto.DepotIdSource);
                // verifica si existe deposito destino
                var depotDestination = await _uow.Depots.GetAsync(entityDto.DepotIdDestination);
                // verifica si existe producto
                var productSource = await _uow.Products.GetAsync(entityDto.ProductId);
                // verifica si existe producto deposito origen
                var existsProductSource = await _uow.Products.GetListAsync(filter: x => x.DepotId == entityDto.DepotIdSource && x.Id == entityDto.ProductId);
                if( productSource == null || existsProductSource == null || 
                    existsProductSource.Count() <= 0 || depotSource == null || depotDestination == null)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                // verifica si existe producto deposito Destino
                Product productDestination = new();
                var existsProductDestination = await _uow.Products.GetListAsync(filter: x => x.DepotId == entityDto.DepotIdDestination && x.Id == entityDto.ProductId);
                if (existsProductDestination == null || existsProductDestination.Count() <= 0)
                {
                    // creo producto con deposito destino
                    productDestination.Id = 0;
                    productDestination.Code = productSource.Code;
                    productDestination.Description = productSource.Description;
                    productDestination.DepotName = depotDestination.Name;
                    productDestination.DepotId = entityDto.DepotIdDestination;
                    productDestination.Quantity = entityDto.Quantity;
                    productDestination.UserId = entityDto.UserId;
                    productDestination.Created = DateTime.Now;
                    productDestination.Modified = DateTime.Now;
                    var product = await _uow.Products.InsertAsync(productDestination);
                    if(product == null)
                    {
                        response.Code = ErrorMessage.IERR_CREATE_REGISTER;
                        response.Message = ErrorMessage.SERR_CREATE_REGISTER;
                        return response;
                    }
                }
                else
                {
                    // actualizo producto con deposito destino
                    productDestination = existsProductDestination.First();
                    productDestination.Quantity += entityDto.Quantity;
                    productDestination.Modified = DateTime.Now;
                    productDestination.UserId = entityDto.UserId;
                    var updateD = await _uow.Products.UpdateAsync(productDestination);
                    if (updateD <= 0)
                    {
                        response.Code = ErrorMessage.IERR_UPDATE_REGISTER;
                        response.Message = ErrorMessage.SERR_UPDATE_REGISTER;
                        return response;
                    }
                }
                // actualizo producto con deposito origen
                productSource.Quantity -= entityDto.Quantity;
                productSource.UserId = entityDto.UserId;
                productSource.Modified = DateTime.Now;
                var update = await _uow.Products.UpdateAsync(productSource);
                if (update <= 0)
                {
                    response.Code = ErrorMessage.IERR_UPDATE_REGISTER;
                    response.Message = ErrorMessage.SERR_UPDATE_REGISTER;
                    return response;
                }
                // creo registro en ProductMovement
                var entity = _mapper.Map<ProductMovement>(entityDto);
                entity.Created = DateTime.Now;
                entity.Modified = DateTime.Now;
                var result = await _uow.ProductMovements.InsertAsync(entity);
                if (result == null)
                {
                    response.Code = ErrorMessage.IERR_CREATE_REGISTER;
                    response.Message = ErrorMessage.SERR_CREATE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<ProductMovementDto>(entity);
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
        public async Task<Response<ProductMovementDto>> UpdateAsync(ProductMovementDto entityDto)
        {
            var response = new Response<ProductMovementDto>();
            if (entityDto == null || entityDto.User == null || entityDto.Product == null)
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.ProductMovements.GetListAsync(filter:x => x.Id == entityDto.Id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                var entity = _mapper.Map<ProductMovement>(entityDto);
                entity.Modified = DateTime.Now;
                int result = await _uow.ProductMovements.UpdateAsync(entity);
                if (result == 0)
                {
                    response.Code = ErrorMessage.IERR_UPDATE_REGISTER;
                    response.Message = ErrorMessage.SERR_UPDATE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<ProductMovementDto>(entity);
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
                var exists = await _uow.ProductMovements.GetListAsync(filter:x => x.Id == id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                int result = await _uow.ProductMovements.DeleteAsync(exists.First());
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
