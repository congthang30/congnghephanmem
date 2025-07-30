using Microsoft.AspNetCore.Mvc;
using EasyBuy.Models;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.NVKD.Controllers
{
    [Area("NVKD")]
    [AuthorizeRole("NVKD")]
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
                // Lấy thống kê tổng quan cho NVKD
                var today = DateTime.Today;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Đếm đơn hàng mới hôm nay (Chờ xác nhận)
                var newOrdersToday = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today && o.Status == "Chờ xác nhận")
                    .CountAsync();

                // Đếm đơn hàng đã xác nhận hôm nay (Đã xác nhận)
                var confirmedOrdersToday = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today && o.Status == "Đã xác nhận")
                    .CountAsync();

                // Đếm đơn hàng mới trong tháng (Chờ xác nhận)
                var newOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Chờ xác nhận")
                    .CountAsync();

                // Đếm đơn hàng đã xác nhận trong tháng (Đã xác nhận)
                var confirmedOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã xác nhận")
                    .CountAsync();

                // Tính tổng doanh thu tháng này (từ đơn hàng đã xác nhận)
                var totalRevenueThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã xác nhận")
                    .SumAsync(o => o.TotalAmount ?? 0);

                // Lấy 5 đơn hàng mới nhất (Chờ xác nhận)
                var recentOrders = await _context.Orders
                    .Include(o => o.User)
                    .Where(o => o.Status == "Chờ xác nhận")
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                // Lấy thống kê đơn hàng theo trạng thái
                var orderStatusStats = await _context.Orders
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                ViewBag.NewOrdersToday = newOrdersToday;
                ViewBag.ConfirmedOrdersToday = confirmedOrdersToday;
                ViewBag.NewOrdersThisMonth = newOrdersThisMonth;
                ViewBag.ConfirmedOrdersThisMonth = confirmedOrdersThisMonth;
                ViewBag.TotalRevenueThisMonth = totalRevenueThisMonth;
                ViewBag.RecentOrders = recentOrders;
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