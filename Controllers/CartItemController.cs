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
        public ActionResult<IEnumerable<CartItemDto>> GetCartItems()
        {
            return Ok(_context.CartItems.ToList());
        }

        [HttpPost]
        public ActionResult<CartItem> AddCartItem([FromBody] AddCartItemData cartItem)
        {
            // Überprüfen, ob der Warenkorb existiert
            var existingCart = _context.Carts.Find(cartItem.CartId);

            if (existingCart == null)
            {
                // Neuen Warenkorb erstellen
                existingCart = new Cart();
                _context.Carts.Add(existingCart);
                _context.SaveChanges(); // Hier wird die neue CartId generiert

                // WICHTIG: Die CartId des cartItem mit der neuen ID überschreiben
                cartItem.CartId = existingCart.Id;
            }

            var existingCartItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartItem.CartId && ci.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                // Neues CartItem erstellen und mit der validen CartId speichern
                existingCartItem = new CartItem
                {
                    CartId = cartItem.CartId, // Hier ist die CartId jetzt sicher gesetzt
                    ProductId = cartItem.ProductId,
                    Quantity = 1
                };
                _context.CartItems.Add(existingCartItem);
            }

            _context.SaveChanges();

            return Ok(existingCartItem); // Das vollständige CartItem zurückgeben
        }


    }
}

public class AddCartItemData
{
    public int ProductId { get; set; }
    public int? CartId { get; set; }
    public int Amount { get; set;  }
}

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int? CartId { get; set; }


}


//public class User
//{
//    public int Id { get; set; }
//    public int login { get; set; }
//    public int password { get; set; }
//    public int email { get; set; }
//    public int name { get; set; }
//}


//public class UserDto
//{
//    public int name { get; set; }
//    public int email { get; set; }
//    public int id { get; set; }
//}

//public class AddUserCommandData
//{
//    public int password { get; set; }
//    public int email { get; set; }
//}
