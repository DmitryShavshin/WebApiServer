using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;
using WebApiServer.DTOs;
using WebApiServer.Interfaces;
using WebApiServer.Models;

namespace WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = _product.GetProducts();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }          
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            try
            {
                var product = _product.GetProductById(id);
                if (product != null)
                    return Ok(product);
                else
                    return NotFound("Product not found");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<Product>> Create(Product request)
        {
            try
            {
                if (ModelState.IsValid)
                    return Ok(await _product.Creeate(request));
                else
                    return BadRequest("Invalid data");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }          
        }


        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<ActionResult<Product>> Update(Product request)
        {
            try
            {
                if (ModelState.IsValid)
                    return Ok(await _product.Update(request));
                else
                    return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }      
        }


        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<ActionResult<Product>> Delete(Product request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _product.Delete(request);
                    return Ok("Product was removed");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}