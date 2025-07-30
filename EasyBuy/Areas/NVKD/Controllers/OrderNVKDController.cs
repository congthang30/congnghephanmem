using EasyBuy.Models;
using EasyBuy.Services.EMAILOTP;
using EasyBuy.Services.MOMO;
using EasyBuy.Services.VNPAY;
using EasyBuy.Attributes;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Areas.NVKD.Controllers
{
    [Area("NVKD")]
    [AuthorizeRole("NVKD")]
    public class OrderNVKDController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly EasyBuyContext _context;
        public OrderNVKDController(IEmailService emailService, EasyBuyContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> ListOrderNew()
        {
            try
            {
                var orders = await _context.Orders
    .Where(o => o.Status == "Chờ xác nhận")
    .Include(o => o.User)
    .Include(o => o.Address)
    .Include(o => o.OrderDetails)
        .ThenInclude(od => od.Product)
    .OrderByDescending(o => o.CreatedAt)
    .ToListAsync();

                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống";
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> ListOrderConfirmed()
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.Status == "Đã xác nhận")
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống";
                Console.WriteLine(ex.Message);
                return View();
            }
        }
  public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.PaymentMethod)
                    .Include(o => o.Voucher)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống";
                Console.WriteLine(ex.Message);
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportInvoice(int orderId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }
                
                var order = await _context.Orders
                  .Include(o => o.OrderDetails)
                      .ThenInclude(od => od.Product)
                  .Include(o => o.User)
                  .Include(o => o.Address)
                  .Include(o => o.PaymentMethod)
                  .Include(o => o.Voucher)
                  .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại" });
                }
                
                var invoice = new Invoice
                {
                    OrderId = order.OrderId,
                    CreatedBy = userId,
                    UpdatedAt = null
                };

                order.Status = "Đã xác nhận";

                _context.Add(invoice);
                await _context.SaveChangesAsync();

                await SendRequestWarehouse(orderId);

                return Json(new { success = true, message = "Tạo hóa đơn thành công!", invoiceId = invoice.InvoiceId });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Có lỗi hệ thống: " + ex.Message });
            }
        }


        private async Task SendRequestWarehouse(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.PaymentMethod)
                    .Include(o => o.Voucher)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    return;

                // Email kho cố định
                var warehouseEmail = "thangcong30x@gmail.com";

                var emailSubject = $"[Yêu cầu xuất kho] Đơn hàng #{order.OrderId}";

                var emailBody = $"Xin chào đội kho,\n\n" +
                    $"Đơn hàng #{order.OrderId} của khách hàng {order.User?.FullName} " +
                    $"đã được xác nhận và cần chuẩn bị đóng gói, xuất kho.\n\n" +
                    $"Địa chỉ giao hàng: {order.Address?.FullAddress}\n" +
                    $"Phương thức thanh toán: {order.PaymentMethod?.MethodName}\n" +
                    $"Voucher: {(order.Voucher != null ? order.Voucher.Code : "Không áp dụng")}\n\n" +
                    $"Danh sách sản phẩm:\n";

                foreach (var item in order.OrderDetails)
                {
                    emailBody += $"- {item.Product?.ProductName} x {item.Quantity}\n";
                }

                emailBody += "\nVui lòng kiểm tra và xử lý sớm nhất.\n\nTrân trọng.";

                await _emailService.SendEmailAsync(warehouseEmail, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email thông báo xuất kho cho đơn hàng #{orderId}: {ex.Message}");
            }
        }
    }
}
