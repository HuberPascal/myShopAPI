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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);

            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product); // gibt die Antwort zurück (201 = created) und auch den link wo es gespeichert wurde, mit der ID (location: https://localhost:7137/api/Product?id=30)
           /* return Ok(product);*/ // gibt die Antwort zurück (201 = 
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Ungültige Produktdaten.");
            }

            var productInDb = await _context.Products.FindAsync(id);
            if (productInDb == null)
            {
                return NotFound($"Produkt mit ID {id} nicht gefunden.");
            }

            // Produkt aktualisieren
            productInDb.Category = product.Category;
            productInDb.Brand = product.Brand;
            productInDb.Name = product.Name;
            productInDb.ProductPrice = product.ProductPrice;
            productInDb.BasePrice = product.BasePrice;
            productInDb.ProductSpecification = product.ProductSpecification;
            productInDb.NumberOfRating = product.NumberOfRating;
            productInDb.ImgSrc = product.ImgSrc;
            productInDb.Rating = product.Rating;

            await _context.SaveChangesAsync();

            // WICHTIG: Gib das aktualisierte Produkt zurück!
            return Ok(productInDb);
        }
    }
}
