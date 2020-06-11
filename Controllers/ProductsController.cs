using AutoMapper;
using BuyProductAPI.Models;
using BuyProductAPI.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BuyProductAPI.Services.ProductService.ProductTypes;

namespace BuyProductAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {

        [HttpGet("{productId}", Name = "GetProduct")]
        //[ProducesResponseType(200, Type = typeof(Product))]  não precisa do  Type = typeof(Product)) devido a utilização do ActionResult<Product>> que informa o tipo de retorna deste método da API
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProduct(
            [FromServices] IProductRepository productRepository,
             [FromServices] IMapper mapper,
            Guid productId)
        {
            var productEntity = await productRepository.GetProduct(productId);

            if (productEntity == null)
                return NotFound();

            var product = mapper.Map<Models.ProductDto>(productEntity);
            return Ok(product);

        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromServices] IProductRepository productRepository,
            [FromServices] IMapper mapper,
            [FromQuery] ProductResourceParameters productResourceParameters)
        {
            var productEntities = await productRepository.GetProducts(productResourceParameters);

            var products = mapper.Map<ICollection<ProductDto>>(productEntities);

            return Ok(products);

        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> CreateProduct(
            [FromServices] IProductRepository productRepository,
            [FromServices] IMapper mapper,
            [FromBody] ProductDto product)
        {
            var productEntity = mapper.Map<Entites.Product>(product);

            if (!TryValidateModel(productEntity))
                return ValidationProblem(ModelState);

            if (!await productRepository.CreateProduct(productEntity))
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao salvar o producto {product.Name}");
                return StatusCode(500, ModelState);
            }

            product.Id = productEntity.Id;
            return CreatedAtRoute("GetProduct", new { productId = productEntity.Id }, product);

        }

        [HttpPut("{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateProduct(
            [FromServices] IProductRepository productRepository,
            [FromServices] IMapper mapper,
             Guid productId,
            [FromBody] ProductDto product)
        {
            var productEntity = await productRepository.GetProduct(productId);

            if (productEntity == null)
                return NotFound();

            mapper.Map(product, productEntity);

            if (!TryValidateModel(productEntity))
                return ValidationProblem(ModelState);

            /* a função map já realiza atualização dos dados do produto que estão no contexto,
             com isso não é necessário usar o método UpdateProduct que faz a mesma função */

            //await productRepository.UpdateProduct(product)

            if (!await productRepository.Save())
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao atualizar o producto {product.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteProduct([FromServices] IProductRepository productRepository, Guid productId)
        {
            var productEntity = await productRepository.GetProduct(productId);

            if (productEntity == null)
                return NotFound();

            if (await productRepository.ExistsProductInAOrder(productId))
            {
                ModelState.AddModelError("", $"Product {productEntity.Name}" +
                      $" não pode ser deletado, porque já faz parte de algu(ns ou m) pedido(s)");
                // código 409 é status de conflito, no caso de chave estrangeira
                return StatusCode(409, ModelState);
            }

            if (!await productRepository.DeleteProduct(productEntity))
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao deletar o producto {productEntity.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
