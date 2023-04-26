using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Domain.Models;
using Domain.Constants;
using Domain.Services;

namespace DepotBlazor.Server.Controllers
{
    /// <summary>
    /// Controlador destinado a la obtención de un JWT token para interactuar con el API.
    /// Es necesario que la compañía se haya dado de alta en el ambiente de la solucón, y se cuente con la APIKey y SecretKey
    /// pedidos para la obtención del token.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        #region Token para acceso
        /// <summary>
        /// Token para acceso
        /// </summary>
        /// <param name="login">Llave de acceso a la API, entregada en el inicio del servicio</param>
        /// <response code="200">Funcionamiento ok. Devuelve objeto estándard de respuesta: Token + Expiración</response>
        /// <response code="400">Bad Request. Indica que faltan parámetros o bien los indicados son incorrectos</response>
        /// <returns></returns>
        [HttpPost("loginAccount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        public async Task<ActionResult<Response<string>>> loginAccount(Login login)
        {
            var response = await _authService.GetToken(login);
            if (response.Code < ErrorMessage.IERR_OK)
            {
                response.Code = StatusCodes.Status400BadRequest;
                response.Data = response.Message;
                return BadRequest(response);
            }
            response.Code = StatusCodes.Status200OK;
            return Ok(response);
        }
        #endregion
    }
}
