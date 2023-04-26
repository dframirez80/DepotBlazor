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
	public class ProductController : ControllerBase
	{
		#region Servicios
		readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		#endregion

		#region HttpGet 
		/// <summary>
		/// Endpoint para obtener todas las entidades.
		/// </summary>
		/// <response code="200">Tarea ejecutada con exito devuelve una lista de actividades.</response>
		/// <response code="401">Se requieren permisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
		/// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
		/// <response code="404">Datos no encontrados.</response>
		[HttpGet]
		[ProducesResponseType(typeof(Response<List<ProductDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(Response<List<ProductDto>>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(Response<List<ProductDto>>), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(Response<List<ProductDto>>), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Response<List<ProductDto>>>> GetAll()
		{
			var resp = await _productService.GetAllAsync();
			if (resp.Code < ErrorMessage.IERR_OK)
			{
				resp.Code = StatusCodes.Status404NotFound;
				return NotFound(resp);
			}
			resp.Code = StatusCodes.Status200OK;
			return Ok(resp);
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
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<Response<ProductDto>>> Create(ProductDto item)
		{
			var resp = new Response<ProductDto>() { Code = StatusCodes.Status400BadRequest, Message = ErrorMessage.SERR_DATA_INVALID };
			if (item == null)
			{
				return BadRequest(resp);
			}
			resp = await _productService.CreateAsync(item);
			if (resp.Code < ErrorMessage.IERR_OK)
			{
				resp.Code = StatusCodes.Status400BadRequest;
				return BadRequest(resp);
			}
			resp.Code = StatusCodes.Status200OK;
			return Ok(resp);
		}
		#endregion

		#region HttpPut
		/// <summary>
		/// Endpoint para actualizar una entidad.
		/// </summary>
		/// <param name="item">Objeto a actualizar.</param>
		/// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
		/// <response code="400">Errores de validacion o excepciones.</response>
		/// <response code="401">Se requieren permisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
		/// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
		[HttpPut]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<Response<ProductDto>>> Update(ProductDto item)
		{
			var resp = new Response<ProductDto>() { Code = StatusCodes.Status400BadRequest, Message = ErrorMessage.SERR_DATA_INVALID };
			if (item == null)
			{
				return BadRequest(resp);
			}
			resp = await _productService.UpdateAsync(item);
			if (resp.Code < ErrorMessage.IERR_OK)
			{
				resp.Code = StatusCodes.Status400BadRequest;
				return BadRequest(resp);
			}
			resp.Code = StatusCodes.Status200OK;
			return Ok(resp);
		}
		#endregion

		#region HttpDelete
		/// <summary>
		/// Eliminar una entidad existente
		/// </summary>
		/// <param name="id">identificador</param>
		/// <response code="200">OK. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
		/// <response code="400">Errores de validacion o excepciones.</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(Response<bool>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Response<bool>>> Delete(int id)
		{
			var resp = new Response<bool>() { Code = StatusCodes.Status400BadRequest, Message = ErrorMessage.SERR_DATA_INVALID, Data = false };
			if (id <= 0)
			{
				return BadRequest(resp);
			}
			resp = await _productService.DeleteAsync(id);
			if (resp.Code < ErrorMessage.IERR_OK)
			{
				return BadRequest(resp);
			}
			resp.Data = true;
			resp.Code = StatusCodes.Status200OK;
			return Ok(resp);
		}
        #endregion

    }
}
