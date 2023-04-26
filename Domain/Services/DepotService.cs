using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;

namespace Domain.Services
{
    public class DepotService : IDepotService
    {
        #region Variables y constructor
        private readonly IUoW _uow;
        private readonly IMapper _mapper;
        public DepotService(IUoW uow, IMapper mapper)
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
        public async Task<Response<List<DepotDto>>> GetAllAsync()
        {
            var response = new Response<List<DepotDto>>();
            try
            {
                var list = await _uow.Depots.GetsAsync();
                if (list.Count() == 0 || list == null)
                {
                    response.Code = ErrorMessage.IERR_WITHOUT_REGISTER;
                    response.Message = ErrorMessage.SERR_WITHOUT_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<List<DepotDto>>(list);
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
        public async Task<Response<DepotDto>> CreateAsync(DepotDto entityDto)
        {
            var response = new Response<DepotDto>();
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Name) || string.IsNullOrEmpty(entityDto.Code))
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.Depots.GetListAsync(filter: x => x.Name == entityDto.Name);
                if (exists != null && exists.Count() > 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_EXIST;
                    response.Message = ErrorMessage.SERR_IDENTITY_EXIST;
                    return response;
                }
                var entity = _mapper.Map<Depot>(entityDto);
                entity.Created = DateTime.Now;
                entity.Modified = DateTime.Now;
                var result = await _uow.Depots.InsertAsync(entity);
                if (result == null)
                {
                    response.Code = ErrorMessage.IERR_CREATE_REGISTER;
                    response.Message = ErrorMessage.SERR_CREATE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<DepotDto>(entity);
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
        public async Task<Response<DepotDto>> UpdateAsync(DepotDto entityDto)
        {
            var response = new Response<DepotDto>();
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Name) || string.IsNullOrEmpty(entityDto.Code))
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                return response;
            }
            try
            {
                var exists = await _uow.Depots.GetListAsync(filter:x => x.Id == entityDto.Id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                var entity = _mapper.Map<Depot>(entityDto);
                entity.Modified = DateTime.Now;
                int result = await _uow.Depots.UpdateAsync(entity);
                if (result == 0)
                {
                    response.Code = ErrorMessage.IERR_UPDATE_REGISTER;
                    response.Message = ErrorMessage.SERR_UPDATE_REGISTER;
                    return response;
                }
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = _mapper.Map<DepotDto>(entity);
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
                var exists = await _uow.Depots.GetListAsync(filter:x => x.Id == id);
                if (exists == null || exists.Count() == 0)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    return response;
                }
                int result = await _uow.Depots.DeleteAsync(exists.First());
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
