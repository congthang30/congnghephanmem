using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Models;


namespace EasyBuy.Areas.NVKT.Controllers
{
    [Area("NVKT")]
    public class ProductsController : Controller
    {
        private readonly EasyBuyContext _context;

        public ProductsController(EasyBuyContext context)
        {
            _context = context;
        }

        public IActionResult ListProducts(
     string? productname,
     string? barcode,
     string? description,
     int? quantity,
     decimal? importprice,
     decimal? sellingprice,
     string? statusproduct,
     decimal? discount,
     bool? isfeatured,
     int? brandid,
     int? cateid)
        {
            var query = _context.Products
                                .Include(p => p.Brand)
                                .Include(p => p.Cate)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(productname))
                query = query.Where(p => p.ProductName.Contains(productname));

            if (!string.IsNullOrWhiteSpace(barcode))
                query = query.Where(p => p.Barcode.Contains(barcode));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(p => p.Description.Contains(description));

            if (quantity.HasValue)
                query = query.Where(p => p.Quantity == quantity.Value);

            if (importprice.HasValue)
                query = query.Where(p => p.ImportPrice == importprice.Value);

            if (sellingprice.HasValue)
                query = query.Where(p => p.SellingPrice == sellingprice.Value);

            if (!string.IsNullOrWhiteSpace(statusproduct))
                query = query.Where(p => p.StatusProduct == statusproduct);

            if (discount.HasValue)
                query = query.Where(p => p.Discount == discount.Value);

            if (isfeatured.HasValue)
                query = query.Where(p => p.IsFeatured == isfeatured.Value);

            if (brandid.HasValue && brandid.Value > 0)
                query = query.Where(p => p.BrandId == brandid.Value);

            if (cateid.HasValue && cateid.Value > 0)
                query = query.Where(p => p.CateId == cateid.Value);

            var filteredProducts = query.ToList();
            ViewBag.ProductName = productname;
            ViewBag.Barcode = barcode;
            ViewBag.Description = description;
            ViewBag.Quantity = quantity;
            ViewBag.ImportPrice = importprice;
            ViewBag.SellingPrice = sellingprice;
            ViewBag.StatusProduct = statusproduct;
            ViewBag.Discount = discount;
            ViewBag.IsFeatured = isfeatured;
            ViewBag.BrandId = brandid;
            ViewBag.CateId = cateid;
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View(filteredProducts);
        }



        public async Task<IActionResult> CreateProducts()
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(string productname, string barcode,
            string? description, int quantity, decimal importprice, decimal sellingprice, string statusproduct, decimal discount,
            bool? isfeatured, IFormFile? imagepr, int brandid, int cateid)
        {
            try
            {
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                var method = new Method.Method();

                if (string.IsNullOrWhiteSpace(productname) ||
                    string.IsNullOrWhiteSpace(barcode) ||
                    string.IsNullOrWhiteSpace(description) ||
                    quantity <= 0 ||
                    importprice <= 0 ||
                    sellingprice <= 0 ||
                    string.IsNullOrWhiteSpace(statusproduct) ||
                    discount < 0 ||
                    brandid <= 0 ||
                    cateid <= 0)
                {
                    ViewBag.Error = "Vui lòng điền đầy đủ và hợp lệ thông tin sản phẩm.";
                    return View();
                }

                if (statusproduct != "hidden" && statusproduct != "presently")
                {
                    ViewBag.Error = "Trạng thái sản phẩm phải là 'hidden' hoặc 'presently'.";
                    return View();
                }
                // Xử lý ảnh upload
                string? imagePath = null;
                try
                {
                    imagePath = await ImageHelper.SaveImageAsync(imagepr, "ratings"); // phương thức trong method ImageHelper để tái sd  
                }
                catch (InvalidDataException ex)
                {
                    TempData["ErrorMessage"] = "Lỗi ảnh: " + ex.Message;
                    return View();
                }
                catch (IOException ex)
                {
                    TempData["ErrorMessage"] = "Không thể lưu ảnh lên máy chủ: " + ex.Message;
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
                    UpdatedAt = DateTime.Now,
                    Discount = discount,
                    IsFeatured = isfeatured ?? false,
                    ImagePr = imagePath,
                    BrandId = brandid,
                    CateId = cateid
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("ListProducts");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi hệ thống. Vui lòng thử lại sau.";
                // Bạn có thể log ex.Message ở đây nếu muốn
                return RedirectToAction("ListProducts");
            }
        }


        [HttpGet]
        public IActionResult UpdateProducts(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProducts(int id,string ?productname, string ?barcode,
    string? description, int ?quantity, decimal? importprice, decimal ?sellingprice,
    string ?statusproduct, decimal? discount, bool? isfeatured, IFormFile? imagepr,
    int ?brandid, int? cateid)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            if(productname != null)
            {
                product.ProductName = productname;
            }
            if(barcode != null) {
                product.Barcode = barcode;
            }
            if(description != null)
            {
                product.Description = description;
            }
            if(quantity != null)
            {
                                product.Quantity = quantity;
            }
            if(importprice != null )
            {
                product.ImportPrice = importprice;
            }
            if(sellingprice != null)
            {
                product.SellingPrice = sellingprice;
            }
            if(statusproduct != null)
            {
                product.StatusProduct = statusproduct;
            }
            if(discount != null)
            {
                product.Discount = discount;
            }
            if(isfeatured != null)
            {
                product.IsFeatured = isfeatured ?? false;
            }
            if(brandid != null)
            {
                product.BrandId = brandid;
            }
            if(cateid != null)
            {
                product.CateId = cateid;
            }
            if (imagepr != null)
            {
                string? imagePath = null;
                try
                {
                    imagePath = await ImageHelper.SaveImageAsync(imagepr, "ratings");
                    product.ImagePr = imagePath; // Gán ảnh mới vào thuộc tính của sản phẩm
                }
                catch (InvalidDataException ex)
                {
                    TempData["ErrorMessage"] = "Lỗi ảnh: " + ex.Message;
                    return View(product); // truyền lại sản phẩm để không mất dữ liệu đã nhập
                }
                catch (IOException ex)
                {
                    TempData["ErrorMessage"] = "Không thể lưu ảnh lên máy chủ: " + ex.Message;
                    return View(product);
                }
            }


            // Cập nhật dữ liệu sản phẩm
            product.UpdatedAt = DateTime.Now;




            _context.SaveChangesAsync();
            ViewBag.SuccessMessage = "Cập nhật sản phẩm thành công!";
            return View();
        }




        [HttpPost]
        
        public IActionResult ToggleStatusProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.StatusProduct = product.StatusProduct == "presently" ? "hidden" : "presently";
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thay đổi trạng thái sản phẩm thành công!";
            return RedirectToAction("ListProducts");
        }





        public IActionResult FilterProducts(string category, string brand)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.CateId == int.Parse(category));

            if (!string.IsNullOrEmpty(brand))
                products = products.Where(p => p.BrandId == int.Parse(brand));

            return View("ListProducts", products.Include(p => p.Brand).Include(p => p.Cate).ToList());
        }
    

    }
}