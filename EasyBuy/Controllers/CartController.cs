using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Controllers
{
    public class CartController : Controller
    {
        private readonly EasyBuyContext _context;

        public CartController(EasyBuyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UserCart()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsCheckedOut == false);

            return View(cart?.CartItems?.ToList() ?? new List<CartItem>());
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId,int ? quantity)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.IsCheckedOut == false);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        IsCheckedOut = false,
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm.");
                }

                int addQuantity = quantity ?? 1;
                if (addQuantity <= 0)
                {
                    addQuantity = 1;
                }

                var existingItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += addQuantity;
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        Quantity = addQuantity,
                        UnitPrice = product.SellingPrice ?? 0,
                    };
                    _context.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("UserCart");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return RedirectToAction("TrangChu", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.IsCheckedOut == false);

                if (cart == null)
                {
                    return NotFound("Không tìm thấy giỏ hàng.");
                }

                var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                {
                    return NotFound("Sản phẩm không có trong giỏ hàng.");
                }

                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("UserCart");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.IsCheckedOut == false);

                if (cart == null)
                {
                    return NotFound("Không tìm thấy giỏ hàng.");
                }

                var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                {
                    return NotFound("Sản phẩm không tồn tại trong giỏ hàng.");
                }

                if (quantity <= 0)
                {
                    _context.CartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("UserCart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return View();
            }
        }

    }
}
