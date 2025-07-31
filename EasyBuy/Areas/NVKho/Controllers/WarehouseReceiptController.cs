using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Models;
using EasyBuy.Attributes;

namespace EasyBuy.Areas.NVKho.Controllers
{
    [Area("NVKho")]
    [AuthorizeRole("NVKho", "Admin")]
    public class WarehouseReceiptController : Controller
    {
        private readonly EasyBuyContext _context;

        public WarehouseReceiptController(EasyBuyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListWarehouseReceipt()
        {
            try
            {
                await EnsureTablesExist();

                var warehouseReceipts = await _context.WarehouseReceipts
                    .Include(wr => wr.Staff)
                    .Include(wr => wr.ReceiptDetails)
                    .OrderByDescending(wr => wr.ReceiptDate)
                    .ToListAsync();

                return View(warehouseReceipts);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách phiếu nhập kho.";
                return View(new List<WarehouseReceipt>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateWarehouseReceipt()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.StatusProduct != "hidden")
                    .Select(p => new { p.ProductId, p.ProductName, p.Barcode, p.SellingPrice })
                    .ToListAsync();

                ViewBag.Products = products;
                ViewBag.StaffList = await _context.Users.ToListAsync();
                
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải trang tạo phiếu nhập kho.";
                return RedirectToAction("ListWarehouseReceipt");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWarehouseReceipt(
            string receiptNumber,
            DateTime receiptDate,
            string supplierName,
            string notes,
            List<int> productIds,
            List<string> barcodes,
            List<string> productNames,
            List<int> quantities,
            List<decimal> unitPrices)
        {
            try
            {
                int? createdByStaff = HttpContext.Session.GetInt32("UserID");
                if (createdByStaff == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                await EnsureTablesExist();

                if (string.IsNullOrWhiteSpace(receiptNumber))
                {
                    TempData["ErrorMessage"] = "Số phiếu không được để trống.";
                    return RedirectToAction("CreateWarehouseReceipt");
                }

                if (productIds == null || productIds.Count == 0)
                {
                    TempData["ErrorMessage"] = "Phải có ít nhất một sản phẩm trong phiếu nhập kho.";
                    return RedirectToAction("CreateWarehouseReceipt");
                }

                var existingReceipt = await _context.WarehouseReceipts
                    .FirstOrDefaultAsync(wr => wr.ReceiptNumber == receiptNumber);
                if (existingReceipt != null)
                {
                    TempData["ErrorMessage"] = "Số phiếu đã tồn tại. Vui lòng chọn số phiếu khác.";
                    return RedirectToAction("CreateWarehouseReceipt");
                }

                var warehouseReceipt = new Models.WarehouseReceipt
                {
                    ReceiptNumber = receiptNumber,
                    ReceiptDate = receiptDate,
                    CreatedByStaff = createdByStaff,
                    SupplierName = supplierName ?? "",
                    Notes = notes ?? "",
                    TotalQuantity = quantities.Sum(),
                    TotalAmount = quantities.Zip(unitPrices, (qty, price) => qty * price).Sum()
                };

                _context.WarehouseReceipts.Add(warehouseReceipt);
                await _context.SaveChangesAsync();

                for (int i = 0; i < productIds.Count; i++)
                {
                    var detail = new WarehouseReceiptDetail
                    {
                        ReceiptID = warehouseReceipt.ReceiptID,
                        Barcode = barcodes[i],
                        ProductName = productNames[i],
                        Quantity = quantities[i],
                        UnitPrice = unitPrices[i]
                    };

                    _context.WarehouseReceiptDetails.Add(detail);
                    var product = await _context.Products
                 .FirstOrDefaultAsync(p => p.ProductId == productIds[i]);

                    if (product != null)
                    {
                        product.Quantity += quantities[i];
                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã tạo phiếu nhập kho {receiptNumber} thành công!";
                return RedirectToAction("ListWarehouseReceipt");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo phiếu nhập kho." +ex;
                return RedirectToAction("CreateWarehouseReceipt");
            }
        }

        private async Task EnsureTablesExist()
        {
            try
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("SELECT TOP 1 * FROM WarehouseReceipts");
                    return;
                }
                catch
                {
                    // Bảng chưa tồn tại, tạo mới
                }

                await _context.Database.ExecuteSqlRawAsync(@"
                    CREATE TABLE WarehouseReceipts (
                        ReceiptID INT IDENTITY(1,1) PRIMARY KEY,
                        ReceiptNumber NVARCHAR(20) NOT NULL,
                        ReceiptDate DATETIME2 NOT NULL,
                        CreatedByStaff INT,
                        SupplierName NVARCHAR(100),
                        Notes NVARCHAR(MAX),
                        TotalQuantity INT NOT NULL,
                        TotalAmount DECIMAL(18,2) NOT NULL
                    )");

                await _context.Database.ExecuteSqlRawAsync(@"
                    CREATE TABLE WarehouseReceiptDetails (
                        DetailID INT IDENTITY(1,1) PRIMARY KEY,
                        ReceiptID INT NOT NULL,
                        Barcode NVARCHAR(50) NOT NULL,
                        ProductName NVARCHAR(100),
                        Quantity INT NOT NULL,
                        UnitPrice DECIMAL(18,2) NOT NULL,
                        TotalPrice AS (Quantity * UnitPrice) PERSISTED
                    )");
            }
            catch (Exception ex)
            {
                // Không throw exception để tránh crash ứng dụng
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetReceiptDetails(int receiptId)
        {
            try
            {
                var receipt = await _context.WarehouseReceipts
                    .Include(wr => wr.Staff)
                    .Include(wr => wr.ReceiptDetails)
                    .FirstOrDefaultAsync(wr => wr.ReceiptID == receiptId);

                if (receipt == null)
                    return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho." });

                var data = new
                {
                    receiptId = receipt.ReceiptID,
                    receiptNumber = receipt.ReceiptNumber,
                    receiptDate = receipt.ReceiptDate.ToString("dd/MM/yyyy HH:mm"),
                    supplierName = receipt.SupplierName,
                    staffName = receipt.Staff?.FullName ?? "Không xác định",
                    totalAmount = receipt.TotalAmount.ToString("N0"),
                    notes = receipt.Notes,
                    receiptDetails = receipt.ReceiptDetails.Select(rd => new
                    {
                        productName = rd.ProductName,
                        barcode = rd.Barcode,
                        quantity = rd.Quantity,
                        unitPrice = rd.UnitPrice.ToString("N0"),
                        totalPrice = rd.TotalPrice.ToString("N0")
                    }).ToList()
                };

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tải chi tiết phiếu nhập kho." });
            }
        }
    }
}
