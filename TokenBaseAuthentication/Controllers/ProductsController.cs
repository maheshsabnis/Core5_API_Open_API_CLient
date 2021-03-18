using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBaseAuthentication.Models;
using TokenBaseAuthentication.Services;

namespace TokenBaseAuthentication.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class ProductsController : ControllerBase
	{
        IRepository<Product, int> repository;
        public ProductsController(IRepository<Product, int> repository)
        {
            this.repository = repository;
        }

        [HttpGet("/get")]
        public IEnumerable<Product> Get()
        {
            var prds = repository.Get();
            return  prds;
        }

        [HttpPost("/createproduct")] 
        public Product Post(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.Create(product);
                return product;
            }
            return null; ;
        }
    }
}
