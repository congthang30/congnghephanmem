using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace EasyBuy.Controllers
{
    public class HomeController : Controller
    {
        private readonly EasyBuyContext _context;
        private EasyBuy.Method.Method method = new EasyBuy.Method.Method();
        public HomeController(EasyBuyContext context)
        {
            _context = context;
        }
        public IActionResult TrangChu(string? search, int? cate, int? brandId, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brands = _context.Brands.ToList();
                var products = _context.Products
                    .Where(p => p.StatusProduct != "hidden" && p.Quantity > 0)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => p.ProductName.Contains(search));
                }

                if (cate.HasValue)
                {
                    products = products.Where(p => p.CateId == cate.Value);
                }

                if (brandId.HasValue)
                {
                    products = products.Where(p => p.BrandId == brandId.Value); 
                }

                if (minPrice.HasValue)
                {
                    products = products.Where(p => p.SellingPrice >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    products = products.Where(p => p.SellingPrice <= maxPrice.Value);
                }

                return View(products.ToList());
            }
            catch (Exception)
            {
                ViewBag.Error = "Có lỗi hệ thống. Vui lòng thử lại sau.";
                return View(new List<Product>());
            }
        }


        [HttpGet]
       public async Task<IActionResult> ViewProductDetails(int productId)
       {
           try
           {
               var userId = HttpContext.Session.GetInt32("UserID");
    
               var detail = await _context.Products
                   .Include(p => p.Ratings.Where(r => r.IsApproved == true))
                   .ThenInclude(r => r.User)
                   .FirstOrDefaultAsync(p => p.ProductId == productId);
    
               if (detail == null)
               {
                   ViewBag.Error = "Không tìm thấy sản phẩm";
                   return RedirectToAction("Error", "NotFoundPage");
               }
               bool existingRating = false;
               bool hasPurchased = false;
               if (userId != null)
               {
                   hasPurchased = await _context.Orders
                       .AnyAsync(o => o.UserId == userId
                                   && o.Status == "Đã giao"
                                   && o.OrderDetails.Any(od => od.ProductId == productId));
                   existingRating = await _context.Ratings
                       .AnyAsync(r => r.ProductId == productId && r.UserId == userId);
               }
               ViewBag.HasPurchased = hasPurchased;
               ViewBag.ExistingRating = existingRating;
    
               detail.ViewCount = (detail.ViewCount ?? 0) + 1;
               await _context.SaveChangesAsync();
    
               return View(detail);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex);
               ViewBag.Error = "Có lỗi hệ thống. Vui lòng thử lại sau";
               return RedirectToAction("Error", "NotFoundPage");
           }
       }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
