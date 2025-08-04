using EasyBuy.Attributes;
using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole("Admin")]
    public class ProductsController : Controller
    {
        private readonly EasyBuyContext _context;

        public ProductsController(EasyBuyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Cate)
                    .OrderByDescending(p => p.UpdatedAt)
                    .ToListAsync();

                // Thống kê
                ViewBag.TotalProducts = await _context.Products.CountAsync();
                ViewBag.ActiveProducts = await _context.Products.Where(p => p.StatusProduct == "presently").CountAsync();
                ViewBag.HiddenProducts = await _context.Products.Where(p => p.StatusProduct == "hidden").CountAsync();
                ViewBag.OutOfStockProducts = await _context.Products.Where(p => p.Quantity <= 0).CountAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách sản phẩm: " + ex.Message;
                return View(new List<Product>());
            }
        }

        public async Task<IActionResult> CreateProducts()
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(string productname, string barcode,
            string? description, int quantity, decimal importprice, decimal sellingprice, 
            string statusproduct, decimal discount, bool? isfeatured, 
            IFormFile? imagepr, int brandid, int cateid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productname) || string.IsNullOrWhiteSpace(barcode) ||
                    quantity <= 0 || importprice <= 0 || sellingprice <= 0 ||
                    string.IsNullOrWhiteSpace(statusproduct) || brandid <= 0 || cateid <= 0)
                {
                    TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin sản phẩm.";
                    ViewBag.Brands = await _context.Brands.ToListAsync();
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View();
                }

                if (await _context.Products.AnyAsync(p => p.Barcode == barcode))
                {
                    TempData["ErrorMessage"] = "Barcode đã tồn tại trong hệ thống.";
                    ViewBag.Brands = await _context.Brands.ToListAsync();
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View();
                }

                var product = new Product
                {
                    ProductName = productname,
                    Barcode = barcode,
                    Description = description,
                    Quantity = quantity,
                    ImportPrice = importprice,
                    SellingPrice = sellingprice,
                    StatusProduct = statusproduct,
                    Discount = discount,
                    IsFeatured = isfeatured ?? false,
                    BrandId = brandid,
                    CateId = cateid,
                    UpdatedAt = DateTime.Now
                };

                if (imagepr != null && imagepr.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagepr.FileName);
                    var filePath = Path.Combine("wwwroot", "images", "products", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagepr.CopyToAsync(stream);
                    }
                    product.ImagePr = "/images/products/" + fileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tạo sản phẩm thành công!";
                return RedirectToAction("ListProducts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Brands = await _context.Brands.ToListAsync();
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View();
            }
        }

        public async Task<IActionResult> UpdateProducts(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProducts(int id, string? productname, string? barcode,
            string? description, int? quantity, decimal? importprice, decimal? sellingprice,
            string? statusproduct, decimal? discount, bool? isfeatured, IFormFile? imagepr,
            int? brandid, int? cateid)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrWhiteSpace(productname))
                    product.ProductName = productname;

                if (!string.IsNullOrWhiteSpace(barcode))
                {
                    if (await _context.Products.AnyAsync(p => p.Barcode == barcode && p.ProductId != id))
                    {
                        TempData["ErrorMessage"] = "Barcode đã tồn tại trong hệ thống.";
                        ViewBag.Brands = await _context.Brands.ToListAsync();
                        ViewBag.Categories = await _context.Categories.ToListAsync();
                        return View(product);
                    }
                    product.Barcode = barcode;
                }

                if (!string.IsNullOrWhiteSpace(description))
                    product.Description = description;

                if (quantity.HasValue)
                    product.Quantity = quantity.Value;

                if (importprice.HasValue)
                    product.ImportPrice = importprice.Value;

                if (sellingprice.HasValue)
                    product.SellingPrice = sellingprice.Value;

                if (!string.IsNullOrWhiteSpace(statusproduct))
                    product.StatusProduct = statusproduct;

                if (discount.HasValue)
                    product.Discount = discount.Value;

                if (isfeatured.HasValue)
                    product.IsFeatured = isfeatured.Value;

                if (brandid.HasValue)
                    product.BrandId = brandid.Value;

                if (cateid.HasValue)
                    product.CateId = cateid.Value;

                if (imagepr != null && imagepr.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagepr.FileName);
                    var filePath = Path.Combine("wwwroot", "images", "products", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagepr.CopyToAsync(stream);
                    }
                    product.ImagePr = "/images/products/" + fileName;
                }

                product.UpdatedAt = DateTime.Now;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("ListProducts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Brands = await _context.Brands.ToListAsync();
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
                return RedirectToAction("ListProducts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa sản phẩm: " + ex.Message;
                return RedirectToAction("ListProducts");
            }
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var data = new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Barcode = product.Barcode,
                Description = product.Description,
                Quantity = product.Quantity,
                ImportPrice = product.ImportPrice,
                SellingPrice = product.SellingPrice,
                StatusProduct = product.StatusProduct,
                Discount = product.Discount,
                IsFeatured = product.IsFeatured,
                BrandName = product.Brand?.NameBrand,
                CategoryName = product.Cate?.CategoryName,
                ImagePr = product.ImagePr,
                UpdatedAt = product.UpdatedAt?.ToString("dd/MM/yyyy")
            };

            return Json(data);
        }
    }
}
