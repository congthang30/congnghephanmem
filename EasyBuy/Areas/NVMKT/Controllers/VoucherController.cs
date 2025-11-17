using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Models;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.NVMKT.Controllers
{
    [Area("NVMKT")]
    [AuthorizeRole("NVMKT", "Admin")]

    public class VoucherController : Controller
    {
        private readonly EasyBuyContext _context;

        public VoucherController(EasyBuyContext context)
        {
            _context = context;
        }

        // 1. Xem danh sách mã giảm giá
        public IActionResult ListVouchers(string? code, string? discountType, bool? isActive)
        {
            var query = _context.Vouchers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(v => v.Code.Contains(code));

            if (!string.IsNullOrWhiteSpace(discountType))
                query = query.Where(v => v.DiscountType == discountType);

            if (isActive.HasValue)
                query = query.Where(v => v.IsActive == isActive);

            var vouchers = query.ToList();

            ViewBag.Code = code;
            ViewBag.DiscountType = discountType;
            ViewBag.IsActive = isActive;

            return View(vouchers);
        }

        // 2. Hiển thị form thêm mới
        public IActionResult CreateVoucher()
        {
            return View();
        }

        // 2. Thêm mã mới (POST)
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(string code,
    string? description,
    string? discountType,
    decimal? discountValue,
    decimal? maxDiscountAmount,
    decimal? minOrderAmount,
    int? quantity,
    DateOnly? startDate,
    DateOnly? endDate,
    bool? isActive,
    bool? isPublic,
    string? createdBy)
        {
            try
            {
                // Validate bắt buộc (ví dụ)
                if (string.IsNullOrWhiteSpace(code))
                {
                    ViewBag.Error = "Mã voucher không được để trống.";
                    return View();
                }
                if (discountValue == null || discountValue <= 0)
                {
                    ViewBag.Error = "Giá trị giảm giá phải lớn hơn 0.";
                    return View();
                }
                if (startDate != null && endDate != null && endDate < startDate)
                {
                    ViewBag.Error = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.";
                    return View();
                }

                var voucher = new Voucher
                {
                    Code = code.Trim(),
                    Description = description,
                    DiscountType = discountType,
                    DiscountValue = discountValue,
                    MaxDiscountAmount = maxDiscountAmount,
                    MinOrderAmount = minOrderAmount,
                    Quantity = quantity,
                    StartDate = startDate,
                    EndDate = endDate,
                    IsActive = isActive ?? true,
                    IsPublic = isPublic ?? true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy
                };

                await _context.Vouchers.AddAsync(voucher);
                await _context.SaveChangesAsync();

                return RedirectToAction("ListVouchers"); // Đổi thành action bạn muốn chuyển tới sau khi tạo
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống. Vui lòng thử lại sau.";
                // Có thể log ex.Message ở đây nếu muốn
                return View();
            }
        }

        // 3. Cập nhật mã - POST
        // GET: Lấy voucher theo id để hiện form cập nhật
        [HttpGet]
        public async Task<IActionResult> UpdateVoucher(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // POST: Cập nhật voucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVoucher(int id,
            string? code,
            string? description,
            string? discountType,
            decimal? discountValue,
            decimal? maxDiscountAmount,
            decimal? minOrderAmount,
            int? quantity,
            DateOnly? startDate,
            DateOnly? endDate,
            bool? isActive,
            bool? isPublic,
            string? createdBy)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }

            // Validate ví dụ đơn giản
            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("code", "Mã voucher không được để trống.");
            }
            if (discountValue == null || discountValue <= 0)
            {
                ModelState.AddModelError("discountValue", "Giá trị giảm giá phải lớn hơn 0.");
            }
            if (startDate != null && endDate != null && endDate < startDate)
            {
                ModelState.AddModelError("", "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
            }

            if (!ModelState.IsValid)
            {
                return View(voucher);
            }

            // Cập nhật các trường
            voucher.Code = code?.Trim() ?? voucher.Code;
            voucher.Description = description ?? voucher.Description;
            voucher.DiscountType = discountType ?? voucher.DiscountType;
            voucher.DiscountValue = discountValue ?? voucher.DiscountValue;
            voucher.MaxDiscountAmount = maxDiscountAmount ?? voucher.MaxDiscountAmount;
            voucher.MinOrderAmount = minOrderAmount ?? voucher.MinOrderAmount;
            voucher.Quantity = quantity ?? voucher.Quantity;
            voucher.StartDate = startDate ?? voucher.StartDate;
            voucher.EndDate = endDate ?? voucher.EndDate;
            voucher.IsActive = isActive ?? voucher.IsActive;
            voucher.IsPublic = isPublic ?? voucher.IsPublic;
            voucher.CreatedBy = createdBy ?? voucher.CreatedBy;

            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Cập nhật voucher thành công!";
            return View(voucher);
        }

        // 4. Ẩn mã (IsActive = false)
        [HttpPost]
        public async Task<IActionResult> ToggleVoucherStatus(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy voucher.";
                return RedirectToAction("ListVouchers");
            }

            voucher.IsActive = !(voucher.IsActive ?? false); // Đổi trạng thái
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = voucher.IsActive == true ? "Voucher đã được kích hoạt." : "Voucher đã bị ẩn.";
            return RedirectToAction("ListVouchers");
        }


        // 5. Lọc theo điều kiện (dùng lại View ListVouchers)
        [HttpGet]
        public IActionResult FilterVoucher(bool? isPublic, DateOnly? expiredBefore)
        {
            var query = _context.Vouchers.AsQueryable();

            if (isPublic.HasValue)
                query = query.Where(v => v.IsPublic == isPublic);

            if (expiredBefore.HasValue)
                query = query.Where(v => v.EndDate <= expiredBefore);

            var filtered = query.ToList();

            ViewBag.IsPublic = isPublic;
            ViewBag.ExpiredBefore = expiredBefore;

            return View("ListVouchers", filtered);
        }

        // 6. Xóa mã giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            try
            {
                var voucher = await _context.Vouchers.FindAsync(id);
                if (voucher == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy mã giảm giá.";
                    return RedirectToAction("ListVouchers");
                }

                // Kiểm tra xem có đơn hàng nào đang sử dụng voucher này không
                var hasOrders = await _context.Orders.AnyAsync(o => o.VoucherId == id);
                if (hasOrders)
                {
                    TempData["ErrorMessage"] = "Không thể xóa mã giảm giá này vì đã có đơn hàng sử dụng.";
                    return RedirectToAction("ListVouchers");
                }

                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa mã giảm giá thành công!";
                return RedirectToAction("ListVouchers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa mã giảm giá: " + ex.Message;
                return RedirectToAction("ListVouchers");
            }
        }
    }
}
