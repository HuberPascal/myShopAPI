using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myShopAPI.Data;
using myShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;


namespace myShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CartItem>> GetCartItems() 
        {
            return Ok(_context.CartItems.ToList());
        }

        [HttpPost]
        public ActionResult<CartItem> AddCartItem([FromBody] CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);

            _context.SaveChanges();

            return CreatedAtAction(nameof(AddCartItem), new { id = cartItem.Id }, cartItem);
        }

    }
}
