using Domain.Entities;
using Domain.Models;

namespace Domain.Services
{
    public interface IAuthService
    {
        #region Obtiene el token para login
        /// <summary>
        /// Obtiene el token
        /// </summary>
        /// <param name="login">login</param>
        /// <returns>objeto</returns>
        Task<Response<string>> GetToken(Login login);
        #endregion
    }
}
