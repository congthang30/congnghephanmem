using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Method;

namespace EasyBuy.Controllers
{
    public class RatingController : Controller
    {
        private readonly EasyBuyContext _context;
        public RatingController(EasyBuyContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> PostRating(int productId, int star, string? comment, IFormFile? image)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId is null)
                    return RedirectToAction("Login", "Account");

                // Kiểm tra đã mua hàng
                var hasPurchased = await _context.Orders
                    .AnyAsync(o => o.UserId == userId
                                && o.Status == "Hoàn thành"
                                && o.OrderDetails.Any(od => od.ProductId == productId));

                if (!hasPurchased)
                    return BadRequest("Bạn chưa từng mua sản phẩm này nên không thể đánh giá.");

                // Kiểm tra đã đánh giá chưa
                var existingRating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);

                if (existingRating != null)
                    return BadRequest("Bạn đã đánh giá sản phẩm này rồi.");

                // Xử lý ảnh upload
                string? imagePath = null;
                try
                {
                    imagePath = await ImageHelper.SaveImageAsync(image, "ratings"); // phương thức trong method ImageHelper để tái sd  
                }
                catch (InvalidDataException ex)
                {
                    TempData["ErrorMessage"] = "Lỗi ảnh: " + ex.Message;
                    return RedirectToAction("ViewProductDetails", "Product", new { productId });
                }
                catch (IOException ex)
                {
                    TempData["ErrorMessage"] = "Không thể lưu ảnh lên máy chủ: " + ex.Message;
                    return RedirectToAction("ViewProductDetails", "Product", new { productId });
                }

                // Tạo đánh giá
                var rating = new Rating
                {
                    ProductId = productId,
                    UserId = userId,
                    Star = star,
                    Comment = comment,
                    ImagePath = imagePath,
                    IsApproved = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();

                return RedirectToAction("ViewProductDetails", "Product", new { productId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi hệ thống.Vui lòng thử lại sau";
                return View();
            }
        }
    }
}
