using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepotBlazor.Server.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	[Authorize]
	public class ProductMovementController : ControllerBase
	{
		#region Servicios
		readonly IProductMovementService _productMovementService;

		public ProductMovementController(IProductMovementService productMovementService)
		{
            _productMovementService = productMovementService;
		}
		#endregion

        #region HttpPost
        /// <summary>
        /// Endpoint para crear una entidad.
        /// </summary>
        /// <param name="item">Objeto a crear.</param>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Se requieren permisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Response<ProductMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ProductMovementDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ProductMovementDto>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<ProductMovementDto>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<ProductMovementDto>>> Create(ProductMovementDto item)
        {
            var resp = new Response<ProductMovementDto>() { Code = StatusCodes.Status400BadRequest, Message = ErrorMessage.SERR_DATA_INVALID };
            if (item == null)
            {
                return BadRequest(resp);
            }
            resp = await _productMovementService.CreateAsync(item);
            if (resp.Code < ErrorMessage.IERR_OK)
            {
                resp.Code = StatusCodes.Status400BadRequest;
                return BadRequest(resp);
            }
            resp.Code = StatusCodes.Status200OK;
            return Ok(resp);
        }
        #endregion
    }
}
