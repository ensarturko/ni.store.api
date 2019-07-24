﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ni.Store.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ni.Store.Api.Models.Requests;
using Ni.Store.Api.Models.Responses;

namespace Ni.Store.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/keys")]
    public class StoresController : BaseController
    {
        private readonly ILogger<StoresController> _logger;
        private readonly IStoreService _storeService;

        public StoresController(ILogger<StoresController> logger, IStoreService storeService)
        {
            _logger = logger;
            _storeService = storeService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StoreGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var response = await _storeService.Get(id);

            if (!response.HasError && response.Data != null)
            {
                return Ok(response.Data);
            }

            if (!response.HasError && response.Data == null)
            {
                return NotFound();
            }

            return BadRequest(response.Errors);
        }

        [HttpGet]
        [ProducesResponseType(typeof(StoreGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var response = _storeService.GetAll();

            if (!response.HasError && response.Data != null)
            {
                return Ok(response.Data);
            }

            if (!response.HasError && response.Data == null)
            {
                return NotFound();
            }

            return BadRequest(response.Errors);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(StorePutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] StorePutRequest request)
        {
            var response = await _storeService.Put(id, request);

            if (!response.HasError && response.Data != null)
            {
                return Ok(response.Data);
            }

            if (!response.HasError && response.Data == null)
            {
                return NotFound();
            }

            return BadRequest(response.Errors);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _storeService.Delete(id);
            if (!response.HasError && response.Data.HasValue && response.Data.Value)
            {
                return NoContent();
            }

            if (!response.HasError && response.Data == null)
            {
                return NotFound();
            }

            return BadRequest(response.Errors);
        }
    }
}