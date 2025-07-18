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
        public IActionResult ViewProductDetails(int productId)
        {
            try
            {
                var detail = _context.Products
                           .Include(p => p.Ratings.Where(r => r.IsApproved == true))
                           .ThenInclude(r => r.User)
                           .FirstOrDefault(p => p.ProductId == productId);
                
                // Kiểm tra null trước khi cập nhật
                if (detail == null)
                {
                    ViewBag.Error = "Không tìm thấy sản phẩm";
                    return View("~/Views/Shared/NotFound.cshtml");
                }
                
                // Cập nhật view count
                detail.ViewCount = (detail.ViewCount ?? 0) + 1;
                _context.SaveChanges();
                
                return View(detail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.Error = "Có lỗi hệ thống. Vui lòng thử lại sau";
                return View("~/Views/Shared/Error.cshtml");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
