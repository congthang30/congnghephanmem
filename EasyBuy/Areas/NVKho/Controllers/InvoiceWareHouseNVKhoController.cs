using EasyBuy.Models;
using EasyBuy.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Areas.NVKho.Controllers
{
    [Area("NVKho")]
    [AuthorizeRole("NVKD", "Admin")]
    public class InvoiceWareHouseNVKhoController : Controller
    {
        private readonly EasyBuyContext _context;

        public InvoiceWareHouseNVKhoController(EasyBuyContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ListOrderConfirm()
        {
            var orders = await _context.Orders
                .Where(o => o.Status == "Đã xác nhận")
                .Include(o => o.OrderDetails).Include(o => o.User)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int orderId)
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
                            .ThenInclude(p => p.Brand)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại" });
                }

                var orderData = new
                {
                    orderId = order.OrderId,
                    customerName = order.User?.FullName,
                    customerPhone = order.User?.Phone,
                    customerEmail = order.User?.Email,
                    address = order.Address?.FullAddress,
                    createdAt = order.CreatedAt?.ToString("dd/MM/yyyy HH:mm"),
                    status = order.Status,
                    statusPayment = order.StatusPayment,
                    paymentMethod = order.PaymentMethod?.MethodName,
                    totalAmount = order.TotalAmount,
                    finalTotal = order.FinalTotal ?? order.TotalAmount,
                    voucher = order.Voucher != null ? new
                    {
                        code = order.Voucher.Code,
                        discountAmount = order.TotalAmount - order.FinalTotal
                    } : null,
                    orderDetails = order.OrderDetails.Select(od => new
                    {
                        productName = od.Product?.ProductName,
                        brandName = od.Product?.Brand?.NameBrand,
                        imageUrl = od.Product?.ImagePr,
                        unitPrice = od.UnitPrice,
                        quantity = od.Quantity,
                        totalPrice = (od.UnitPrice ?? 0) * (od.Quantity ?? 0) - (od.Discount ?? 0)
                    }).ToList()
                };

                return Json(new { success = true, data = orderData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeInvoiceWarehouse(int orderId)
        {
            try
            {
                int? staffid = HttpContext.Session.GetInt32("UserID");
                if (staffid == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null) return NotFound();

                var invoice = new InvoiceWareHouse
                {
                    ExportDate = DateTime.Now,
                    UserID = order.UserId.Value,
                    StaffID = staffid.Value,
                    OrderID = order.OrderId,
                    TotalQuantity = order.OrderDetails.Sum(od => od.Quantity ?? 0)
                };

                await _context.InvoiceWareHouses.AddAsync(invoice);

                order.Status = "Chờ giao hàng";

                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo phiếu xuất kho thành công.";
                return RedirectToAction("ListInvoiceWarehouse");
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message;
                Console.WriteLine("DbUpdateException: " + innerMessage);
                return BadRequest("Lỗi CSDL chi tiết: " + innerMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }


        public async Task<IActionResult> DetailsInvoiceWarehouse(int invoiceId)
        {
            var invoice = await _context.InvoiceWareHouses
                .Include(i => i.Order)
                    .ThenInclude(o => o.User)
                .Include(i => i.Order)
                    .ThenInclude(o => o.Address)
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Brand)
                .Include(i => i.Staff)
                .FirstOrDefaultAsync(i => i.InvoiceWareHouseID == invoiceId);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }


        [HttpGet]
        public async Task<IActionResult> GetInvoiceDetails(int invoiceId)
        {
            var invoice = await _context.InvoiceWareHouses
                .Include(i => i.Order)
                    .ThenInclude(o => o.User)
                .Include(i => i.Order)
                    .ThenInclude(o => o.Address)
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Brand)
                .Include(i => i.Staff)
                .FirstOrDefaultAsync(i => i.InvoiceWareHouseID == invoiceId);

            if (invoice == null)
                return Json(new { success = false, message = "Không tìm thấy phiếu xuất kho." });

            var data = new
            {
                invoiceId = invoice.InvoiceWareHouseID,
                createdAt = invoice.ExportDate.ToString("dd/MM/yyyy HH:mm"),
                customerName = invoice.Order?.User?.FullName,
                address = invoice.Order?.Address?.FullAddress,
                phone = invoice.Order?.User?.Phone,
                staffName = invoice.Staff?.FullName,
                products = invoice.Order?.OrderDetails.Select(od => new
                {
                    productName = od.Product?.ProductName,
                    brand = od.Product?.Brand?.NameBrand,
                    quantity = od.Quantity,
                    unitPrice = od.UnitPrice
                })
            };

            return Json(new { success = true, data });
        }


        [HttpGet]
        public async Task<IActionResult> ListInvoiceWarehouse()
        {
            try
            {
                var invoices = await _context.InvoiceWareHouses
                    .Include(i => i.Order)
                        .ThenInclude(o => o.User)
                    .Include(i => i.Staff)
                    .OrderByDescending(i => i.ExportDate)
                    .ToListAsync();

                return View(invoices);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách phiếu xuất kho: " + ex.Message);
                return StatusCode(500, "Có lỗi xảy ra khi tải danh sách phiếu xuất kho.");
            }
        }

        private async Task NotifyShippingDepartment(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null) return;

                // Email bộ phận vận chuyển (có thể cấu hình)
                var shippingEmail = "shipping@easybuy.com"; // Cấu hình trong appsettings.json

                var emailSubject = $"[Yêu cầu vận chuyển] Đơn hàng #{order.OrderId} đã sẵn sàng";

                var emailBody = $"Kính gửi bộ phận vận chuyển,\n\n" +
                    $"Đơn hàng #{order.OrderId} đã được kho chuẩn bị xong và sẵn sàng để vận chuyển.\n\n" +
                    $"Thông tin đơn hàng:\n" +
                    $"- Khách hàng: {order.User?.FullName}\n" +
                    $"- Số điện thoại: {order.User?.Phone}\n" +
                    $"- Địa chỉ giao hàng: {order.Address?.FullAddress}\n" +
                    $"- Số lượng kiện: {order.OrderDetails?.Count()}\n" +
                    $"- Tổng giá trị: {order.FinalTotal ?? order.TotalAmount:N0}₫\n\n" +
                    $"Vui lòng đến kho để nhận hàng và tiến hành vận chuyển.\n\n" +
                    $"Trân trọng,\n" +
                    $"Bộ phận Kho";

                // Uncomment nếu có email service
                // await _emailService.SendEmailAsync(shippingEmail, emailSubject, emailBody);

                Console.WriteLine($"Đã thông báo cho bộ phận vận chuyển về đơn hàng #{orderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi thông báo vận chuyển: {ex.Message}");
            }
        }

    }
}
