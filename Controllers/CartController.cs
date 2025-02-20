using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myShopAPI.Data;
using myShopAPI.Models;


namespace myShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        public ActionResult<Cart> AddCart([FromBody] Cart cart )
        {

            foreach( var item in cart.Items)
            {
                item.Id = 0;
                item.CartId = 0;
                //item.Cart = null;
            }

            _context.Carts.Add(cart);

            _context.SaveChanges();

            return CreatedAtAction(nameof(AddCart), new { id = cart.Id }, cart);
        }
    }
}
