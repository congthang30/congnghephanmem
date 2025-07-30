using EasyBuy.Models;
using EasyBuy.Services.EMAILOTP;
using EasyBuy.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Areas.NVKD.Controllers
{
    [Area("NVKD")]
    [AuthorizeRole("NVKD")]
    public class InvoiceNVKDController : Controller
    {
        private readonly EasyBuyContext _context;
        
        public InvoiceNVKDController(EasyBuyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListInvoice()
        {
            try
            {
                var invoices = await _context.Invoice
                    .Include(i => i.Order)
                        .ThenInclude(o => o.User)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.Address)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.PaymentMethod)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                    .Include(i => i.CreatedByUser)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToListAsync();

                return View(invoices);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống: " + ex.Message;
                Console.WriteLine(ex.Message);
                return View(new List<Invoice>());
            }
        }

        public async Task<IActionResult> DetailsInvoice(int invoiceId)
        {
            try
            {
                var invoice = await _context.Invoice
                    .Include(i => i.Order)
                        .ThenInclude(o => o.User)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.Address)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.PaymentMethod)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.Voucher)
                    .Include(i => i.Order)
                        .ThenInclude(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                    .Include(i => i.CreatedByUser)
                    .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

                if (invoice == null)
                {
                    TempData["Error"] = "Không tìm thấy hóa đơn!";
                    return RedirectToAction("ListInvoice");
                }

                return View(invoice);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi hệ thống: " + ex.Message;
                return RedirectToAction("ListInvoice");
            }
        }
    }
}
