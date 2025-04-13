using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myShopAPI.Data;
using myShopAPI.Models;
using myShopAPI.Transport;


namespace myShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreateCartDto>> CreateCart([FromBody] int userId)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingCart != null)
            {
                return Ok(_mapper.Map<CreateCartDto>(existingCart));
            }

            var cart = new Cart
            {
                UserId = userId
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();


            return Ok(_mapper.Map<CreateCartDto>(cart));
        }

        // [HttpPost]
        // public ActionResult<Cart> AddCart([FromBody] Cart cart)
        // {

        //     foreach (var item in cart.Items)
        //     {
        //         item.Id = 0;
        //         item.CartId = 0;
        //         //item.Cart = null;
        //     }

        //     _context.Carts.Add(cart);

        //     _context.SaveChanges();

        //     return CreatedAtAction(nameof(AddCart), new { id = cart.Id }, cart);
        // }
    }
}
