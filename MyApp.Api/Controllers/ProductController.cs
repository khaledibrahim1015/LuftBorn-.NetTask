using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Commands;
using MyApp.Application.Queries;
using MyApp.Application.Response;
using MyApp.Domain.Specifications;
using System.Net;

namespace MyApp.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost]
        [Route("PaginatedProducts")]
        [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromBody] PaginatedSpecsParams paginatedSpecsParams)
        {
            try
            {
                _logger.LogInformation("Entering GetAllProducts with PageIndex={PageIndex}, PageSize={PageSize}, Search={Search}",
          paginatedSpecsParams.PageIndex, paginatedSpecsParams.PageSize, paginatedSpecsParams.Search);
                var query = new GetPaginatedProductsQuery(paginatedSpecsParams);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetAllProducts()
        //{

        //    _logger.LogInformation("Getting all products");
        //    var query = new GetAllProductQuery();
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation("Getting product with ID {ProductId}", id);
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            _logger.LogInformation("Creating a new product");
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProductById), new { id = result }, null);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                _logger.LogWarning("Product ID mismatch: {ProductId} != {CommandId}", id, command.Id);
                return BadRequest("Product ID mismatch");
            }

            _logger.LogInformation("Updating product with ID {ProductId}", id);
            var result = await _mediator.Send(command);
            if (result == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation("Deleting product with ID {ProductId}", id);
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            return NoContent();
        }
    }
}
