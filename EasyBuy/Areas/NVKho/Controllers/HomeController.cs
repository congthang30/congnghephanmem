using Microsoft.AspNetCore.Mvc;
using EasyBuy.Models;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.NVKho.Controllers
{
    [Area("NVKho")]
    [AuthorizeRole("NVKho", "Admin")]
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
                // Lấy thống kê tổng quan cho NVKho
                var today = DateTime.Today;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Đếm đơn hàng chờ xử lý hôm nay
                var pendingOrdersToday = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today && o.Status == "Đã xác nhận")
                    .CountAsync();

                // Đếm đơn hàng đã xử lý hôm nay
                var processedOrdersToday = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today && o.Status == "Đã giao")
                    .CountAsync();

                // Đếm đơn hàng chờ xử lý trong tháng
                var pendingOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã xác nhận")
                    .CountAsync();

                // Đếm đơn hàng đã xử lý trong tháng
                var processedOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã giao")
                    .CountAsync();

                // Tính tổng doanh thu tháng này (từ đơn hàng đã giao)
                var totalRevenueThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã giao")
                    .SumAsync(o => o.TotalAmount ?? 0);

                // Lấy 5 đơn hàng chờ xử lý mới nhất
                var recentPendingOrders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Where(o => o.Status == "Đã xác nhận")
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                // Lấy thống kê sản phẩm tồn kho
                var lowStockProducts = await _context.Products
                    .Where(p => p.Quantity <= 10 && p.StatusProduct != "hidden")
                    .Take(5)
                    .ToListAsync();

                // Lấy thống kê đơn hàng theo trạng thái
                var orderStatusStats = await _context.Orders
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                // Thống kê sản phẩm theo danh mục
                var categoryStats = await _context.Products
                    .Include(p => p.Cate)
                    .Where(p => p.Cate != null)
                    .GroupBy(p => p.Cate.CategoryName)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .Take(5)
                    .ToListAsync();

                ViewBag.PendingOrdersToday = pendingOrdersToday;
                ViewBag.ProcessedOrdersToday = processedOrdersToday;
                ViewBag.PendingOrdersThisMonth = pendingOrdersThisMonth;
                ViewBag.ProcessedOrdersThisMonth = processedOrdersThisMonth;
                ViewBag.TotalRevenueThisMonth = totalRevenueThisMonth;
                ViewBag.RecentPendingOrders = recentPendingOrders;
                ViewBag.LowStockProducts = lowStockProducts;
                ViewBag.OrderStatusStats = orderStatusStats;
                ViewBag.CategoryStats = categoryStats;

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