using EasyBuy.Attributes;
using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole("Admin")]
    public class UsersController : Controller
    {
        private readonly EasyBuyContext _context;

        public UsersController(EasyBuyContext context)
        {
            _context = context;
        }

        public IActionResult ListUsers()
        {
            var listuser = _context.Users.ToList();
            return View(listuser);
        }

        [HttpPost]
        public IActionResult EditUser(int userid, string? name, string? password, string? phone,
                                string? accountstatus, string? email, string? role)
        {
            try
            {
                var user = _context.Users.Find(userid);
                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng");
                }

                if (name != null)
                {
                    user.FullName = name;
                }
                if (password != null)
                {
                    user.Password = password;
                }
                if (phone != null)
                {
                    if (_context.Users.Any(u => u.Phone == phone && u.UserId != userid))
                    {
                        return BadRequest("SDT đã được sử dụng bởi người dùng khác.");
                    }
                    user.Phone = phone;
                }
                if (accountstatus != null)
                {
                    user.AccountStatus = accountstatus;
                }
                if (email != null)
                {
                    if (_context.Users.Any(u => u.Email == email && u.UserId != userid))
                    {
                        return BadRequest("Email đã được sử dụng bởi người dùng khác.");
                    }
                    user.Email = email;
                }
                if (role != null)
                {
                    user.Role = role;
                }
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("ListUsers", "Users", new { area = "Admin" });

            }
            catch
            {
                return BadRequest("Đã có lỗi xảy ra trong quá trình cập nhật người dùng. Vui lòng thử lại sau.");
            }
        }
        [HttpPost]
        public IActionResult DeleteUser(int userid)
        {
            try
            {
                if (userid <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ");
                }
                var user = _context.Users.Find(userid);
                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng");
                }
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("ListUsers", "Users", new { area = "Admin" });
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = "Không thể xóa người dùng vì đang được sử dụng ở bảng khác.";
                return RedirectToAction("ListUsers", "Users", new { area = "Admin" });
            }
            catch
            {
                return BadRequest("Đã có lỗi xảy ra trong quá trình xóa người dùng. Vui lòng thử lại sau.");
            }
        }
        [HttpPost]
        public IActionResult CreateUser(string name, string password, string phone,
                                string accountstatus, string email, string role)
        {
            try
            {
                if (_context.Users.Any(u => u.Phone == phone))
                {
                    return BadRequest("Số điện thoại đã được sử dụng bởi người dùng khác.");
                }
                if (_context.Users.Any(u => u.Email == email))
                {
                    return BadRequest("Email đã được sử dụng bởi người dùng khác.");
                }
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone) ||
                    string.IsNullOrEmpty(accountstatus) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
                {
                    return BadRequest("Vui lòng điền đầy đủ thông tin người dùng.");
                }
                var newUser = new User
                {
                    FullName = name,
                    Password = password,
                    Phone = phone,
                    AccountStatus = accountstatus,
                    Email = email,
                    Role = role,
                    FailedLoginCount = 0,
                    LockedAt = null,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("ListUsers", "Users", new { area = "Admin" });
            }
            catch
            {
                return BadRequest("Đã có lỗi xảy ra trong quá trình tạo người dùng. Vui lòng thử lại sau.");
            }
        }
        public IActionResult UserDetail(int userid)
        {
            if (userid <= 0)
            {
                return BadRequest("ID không hợp lệ.");
            }

            var user = _context.Users.Find(userid);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            var data = new
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Phone = user.Phone,
                AccountStatus = user.AccountStatus,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt?.ToString("dd/MM/yyyy"),
                FailedLoginCount = user.FailedLoginCount,
                LockedAt = user.LockedAt?.ToString("dd/MM/yyyy")
            };

            return Json(data);
        }

    }
}
