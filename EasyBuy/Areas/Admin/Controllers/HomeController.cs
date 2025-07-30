using Microsoft.AspNetCore.Mvc;
using EasyBuy.Models;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole("Admin")]
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
                var today = DateTime.Today;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Thống kê tổng quan hệ thống
                var totalUsers = await _context.Users.CountAsync();
                var totalProducts = await _context.Products.Where(p => p.StatusProduct != "hidden").CountAsync();
                var totalOrders = await _context.Orders.CountAsync();
                var totalRevenue = await _context.Orders
                    .Where(o => o.Status == "Đã giao")
                    .SumAsync(o => o.TotalAmount ?? 0);

                // Thống kê theo thời gian
                var newUsersThisMonth = await _context.Users
                    .Where(u => u.CreatedAt.HasValue && u.CreatedAt.Value.Month == currentMonth && u.CreatedAt.Value.Year == currentYear)
                    .CountAsync();
                var newOrdersThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear)
                    .CountAsync();
                var revenueThisMonth = await _context.Orders
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == currentMonth && o.CreatedAt.Value.Year == currentYear && o.Status == "Đã giao")
                    .SumAsync(o => o.TotalAmount ?? 0);

                // Thống kê đơn hàng theo trạng thái
                var pendingOrders = await _context.Orders.Where(o => o.Status == "Chờ xác nhận").CountAsync();
                var confirmedOrders = await _context.Orders.Where(o => o.Status == "Đã xác nhận").CountAsync();
                var deliveredOrders = await _context.Orders.Where(o => o.Status == "Đã giao").CountAsync();
                var cancelledOrders = await _context.Orders.Where(o => o.Status == "Đã hủy").CountAsync();

                // Thống kê sản phẩm
                var outOfStockProducts = await _context.Products.Where(p => p.Quantity <= 0 && p.StatusProduct != "hidden").CountAsync();
                var lowStockProducts = await _context.Products.Where(p => p.Quantity <= 10 && p.Quantity > 0 && p.StatusProduct != "hidden").CountAsync();
                var featuredProducts = await _context.Products.Where(p => p.IsFeatured == true && p.StatusProduct != "hidden").CountAsync();

                // Thống kê người dùng theo vai trò
                var adminUsers = await _context.Users.Where(u => u.Role == "Admin").CountAsync();
                var nvkdUsers = await _context.Users.Where(u => u.Role == "NVKD").CountAsync();
                var nvkhoUsers = await _context.Users.Where(u => u.Role == "NVKho").CountAsync();
                var nvmktUsers = await _context.Users.Where(u => u.Role == "NVMKT").CountAsync();
                var customerUsers = await _context.Users.Where(u => u.Role == "Customer").CountAsync();

                // Đơn hàng gần đây
                var recentOrders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                // Sản phẩm bán chạy (theo lượt xem)
                var topViewedProducts = await _context.Products
                    .Where(p => p.StatusProduct != "hidden")
                    .OrderByDescending(p => p.ViewCount)
                    .Take(5)
                    .ToListAsync();

                // Thống kê doanh thu theo tháng (6 tháng gần nhất)
                var monthlyRevenue = new List<object>();
                for (int i = 5; i >= 0; i--)
                {
                    var month = DateTime.Now.AddMonths(-i);
                    var revenue = await _context.Orders
                        .Where(o => o.CreatedAt.HasValue && 
                                   o.CreatedAt.Value.Month == month.Month && 
                                   o.CreatedAt.Value.Year == month.Year && 
                                   o.Status == "Đã giao")
                        .SumAsync(o => o.TotalAmount ?? 0);
                    monthlyRevenue.Add(new { Month = month.ToString("MM/yyyy"), Revenue = revenue });
                }

                // Thống kê danh mục sản phẩm
                var categoryStats = await _context.Products
                    .Include(p => p.Cate)
                    .Where(p => p.Cate != null && p.StatusProduct != "hidden")
                    .GroupBy(p => p.Cate.CategoryName)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .Take(5)
                    .ToListAsync();

                // Thống kê thương hiệu
                var brandStats = await _context.Products
                    .Include(p => p.Brand)
                    .Where(p => p.Brand != null && p.StatusProduct != "hidden")
                    .GroupBy(p => p.Brand.NameBrand)
                    .Select(g => new { Brand = g.Key, Count = g.Count() })
                    .Take(5)
                    .ToListAsync();

                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalProducts = totalProducts;
                ViewBag.TotalOrders = totalOrders;
                ViewBag.TotalRevenue = totalRevenue;
                ViewBag.NewUsersThisMonth = newUsersThisMonth;
                ViewBag.NewOrdersThisMonth = newOrdersThisMonth;
                ViewBag.RevenueThisMonth = revenueThisMonth;
                ViewBag.PendingOrders = pendingOrders;
                ViewBag.ConfirmedOrders = confirmedOrders;
                ViewBag.DeliveredOrders = deliveredOrders;
                ViewBag.CancelledOrders = cancelledOrders;
                ViewBag.OutOfStockProducts = outOfStockProducts;
                ViewBag.LowStockProducts = lowStockProducts;
                ViewBag.FeaturedProducts = featuredProducts;
                ViewBag.AdminUsers = adminUsers;
                ViewBag.NVKDUsers = nvkdUsers;
                ViewBag.NVKhoUsers = nvkhoUsers;
                ViewBag.NVMKTUsers = nvmktUsers;
                ViewBag.CustomerUsers = customerUsers;
                ViewBag.RecentOrders = recentOrders;
                ViewBag.TopViewedProducts = topViewedProducts;
                ViewBag.MonthlyRevenue = monthlyRevenue;
                ViewBag.CategoryStats = categoryStats;
                ViewBag.BrandStats = brandStats;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }
    }
}
