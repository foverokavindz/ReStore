using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductController(StoreContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public async Task <ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return  Ok(products);
        }

    // parameter id is passed to the method as a route parameter
        [HttpGet("{id}")] // api/products/3
        public async Task <ActionResult <Product>> GetReport(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }

     
}