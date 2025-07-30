using EasyBuy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Controllers
{
    public class WishlistController : Controller
    {
        private readonly EasyBuyContext _context;
        public WishlistController(EasyBuyContext context)
        {
            _context = context;
        }
        public IActionResult Wishlist()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                    return RedirectToAction("Login", "Account");
                var wishlist = _context.Wishlists
                             .Include(w => w.Product)
                             .Where(w => w.UserId == userId)
                             .Select(w => w.Product)
                             .ToList();
                return View(wishlist);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddWishList(int productId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                    return RedirectToAction("Login", "Account");
                var existingItem = await _context.Wishlists
                        .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                if (existingItem == null)
                {
                    var wishlist = new Wishlist
                    {
                        ProductId = productId,
                        UserId = userId.Value,
                        CreatedAt = DateTime.Now,
                    };
                    _context.Wishlists.Add(wishlist);
                    TempData["Message"] = "Đã thêm vào yêu thích!";
                }
                else
                {
                    _context.Wishlists.Remove(existingItem);
                    TempData["Message"] = "Đã xóa khỏi yêu thích!";
                }

                await _context.SaveChangesAsync();

                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWishListAjax(int productId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để sử dụng tính năng này", requireLogin = true });
                }

                var existingItem = await _context.Wishlists
                        .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                bool added = false;
                if (existingItem == null)
                {
                    var wishlist = new Wishlist
                    {
                        ProductId = productId,
                        UserId = userId.Value,
                        CreatedAt = DateTime.Now,
                    };
                    _context.Wishlists.Add(wishlist);
                    added = true;
                }
                else
                {
                    _context.Wishlists.Remove(existingItem);
                    added = false;
                }

                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    added = added,
                    message = added ? "Đã thêm vào yêu thích!" : "Đã xóa khỏi yêu thích!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi hệ thống. Vui lòng thử lại sau" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveWishList(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Đã xóa khỏi yêu thích!";
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> CheckWishlistStatus(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { inWishlist = false });
            }

            var existingItem = await _context.Wishlists
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            return Json(new { inWishlist = existingItem != null });
        }

    }
}
