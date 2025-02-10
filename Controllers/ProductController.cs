using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myShopAPI.Data;
using myShopAPI.Models;

namespace myShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);

            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product ); // gibt die Antwort zurück (201 = created) und auch den link wo es gespeichert wurde, mit der ID (location: https://localhost:7137/api/Product?id=30)
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_context.Products.ToList());
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if(product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            var productInDb = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            //productInDb.Name = product.Name;
            //productInDb.Price = product.Price;

            _context.SaveChanges();

            return Ok();
        }
    }
}
