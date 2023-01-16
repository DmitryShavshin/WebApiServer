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
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<Product>> Create(Product request)
        {
            if(ModelState.IsValid)
            {
                var result = _product.Creeate(request);
                return Ok(result);
            }
            return BadRequest("Invalid data");
        }


        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<ActionResult<Product>> Update(Product request)
        {
            if (ModelState.IsValid)
            {
                var result = _product.Update(request);
                return Ok(result);  
            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<ActionResult<Product>> Delete(Product request)
        {
            if (ModelState.IsValid)
            {
                var result = _product.Delete(request);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}