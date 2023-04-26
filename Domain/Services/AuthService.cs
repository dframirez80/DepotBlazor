using AutoMapper;
using Domain.Constants;
using Domain.Helpers;
using Domain.Models;
using Domain.Repository;
using Domain.Security;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using User = Domain.Entities.User;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        #region Variables y constructor
        private readonly IUoW _uow;
        private readonly IMapper _mapper;
        private readonly ISecurityJwt _securityJwt;

        public AuthService(IUoW uow, IMapper mapper,
                           ISecurityJwt securityJwt)
        {
            _uow = uow;
            _mapper = mapper;
            _securityJwt = securityJwt;
        }
        #endregion

        #region Obtiene el token para login
        /// <summary>
        /// Obtiene el token
        /// </summary>
        /// <param name="login">login</param>
        /// <returns>objeto</returns>
        public async Task<Response<string>> GetToken(Login login)
        {
            var response = new Response<string>();
            if (login == null)
            {
                response.Code = ErrorMessage.IERR_DATA_INVALID;
                response.Message = ErrorMessage.SERR_DATA_INVALID;
                response.Data = string.Empty;
                return response;
            }
            try
            {
                var users = await _uow.Users.GetListAsync(filter:x => x.Name == login.Email);
                var user = users.FirstOrDefault();
                if (user == null)
                {
                    response.Code = ErrorMessage.IERR_IDENTITY_NOT_FOUND;
                    response.Message = ErrorMessage.SERR_IDENTITY_NOT_FOUND;
                    response.Data = string.Empty;
                    return response;
                }
                if (user.Password != TripleDES.Encrypt(login.Password))
                {
                    response.Code = ErrorMessage.IERR_DATA_INVALID;
                    response.Message = ErrorMessage.SERR_DATA_INVALID;
                    return response;
                }
                int expiresMinutes = _securityJwt.GetMinuteJwtExpiration();
                string token = _securityJwt.GenerateToken(user, expiresMinutes);
                response.Code = ErrorMessage.IERR_OK;
                response.Message = ErrorMessage.SERR_OK;
                response.Data = token;
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
