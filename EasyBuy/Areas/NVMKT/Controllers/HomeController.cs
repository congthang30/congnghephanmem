using Microsoft.AspNetCore.Mvc;
using EasyBuy.Models;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.NVMKT.Controllers
{
    [Area("NVMKT")]
    [AuthorizeRole("NVMKT")]
    public class HomeController : Controller
    {
        private readonly EasyBuyContext _context;

        public HomeController(EasyBuyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy thống kê tổng quan cho NVMKT
                var today = DateTime.Today;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Đếm tổng số sản phẩm
                var totalProducts = await _context.Products
                    .Where(p => p.StatusProduct != "hidden")
                    .CountAsync();

                // Đếm sản phẩm nổi bật
                var featuredProducts = await _context.Products
                    .Where(p => p.IsFeatured == true && p.StatusProduct != "hidden")
                    .CountAsync();

                // Đếm sản phẩm hết hàng
                var outOfStockProducts = await _context.Products
                    .Where(p => p.Quantity <= 0 && p.StatusProduct != "hidden")
                    .CountAsync();

                // Đếm sản phẩm sắp hết hàng (≤ 10)
                var lowStockProducts = await _context.Products
                    .Where(p => p.Quantity <= 10 && p.Quantity > 0 && p.StatusProduct != "hidden")
                    .CountAsync();

                // Lấy 5 sản phẩm có lượt xem cao nhất
                var topViewedProducts = await _context.Products
                    .Where(p => p.StatusProduct != "hidden")
                    .OrderByDescending(p => p.ViewCount)
                    .Take(5)
                    .ToListAsync();

                // Lấy 5 sản phẩm nổi bật
                var featuredProductsList = await _context.Products
                    .Where(p => p.IsFeatured == true && p.StatusProduct != "hidden")
                    .Take(5)
                    .ToListAsync();

                // Lấy 5 sản phẩm sắp hết hàng
                var lowStockProductsList = await _context.Products
                    .Where(p => p.Quantity <= 10 && p.Quantity > 0 && p.StatusProduct != "hidden")
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .ToListAsync();

                // Thống kê sản phẩm theo danh mục
                var categoryStats = await _context.Products
                    .Include(p => p.Cate)
                    .Where(p => p.Cate != null && p.StatusProduct != "hidden")
                    .GroupBy(p => p.Cate.CategoryName)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .Take(5)
                    .ToListAsync();

                // Thống kê sản phẩm theo thương hiệu
                var brandStats = await _context.Products
                    .Include(p => p.Brand)
                    .Where(p => p.Brand != null && p.StatusProduct != "hidden")
                    .GroupBy(p => p.Brand.NameBrand)
                    .Select(g => new { Brand = g.Key, Count = g.Count() })
                    .Take(5)
                    .ToListAsync();

                // Thống kê đơn hàng theo trạng thái
                var orderStatusStats = await _context.Orders
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                ViewBag.TotalProducts = totalProducts;
                ViewBag.FeaturedProducts = featuredProducts;
                ViewBag.OutOfStockProducts = outOfStockProducts;
                ViewBag.LowStockProducts = lowStockProducts;
                ViewBag.TopViewedProducts = topViewedProducts;
                ViewBag.FeaturedProductsList = featuredProductsList;
                ViewBag.LowStockProductsList = lowStockProductsList;
                ViewBag.CategoryStats = categoryStats;
                ViewBag.BrandStats = brandStats;
                ViewBag.OrderStatusStats = orderStatusStats;

                return View();
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                ViewBag.Error = "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }
    }
}
