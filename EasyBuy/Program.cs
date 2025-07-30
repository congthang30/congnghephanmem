using EasyBuy.Models;
using EasyBuy.Services.EMAILOTP;
using EasyBuy.Services.VNPAY;
using EasyBuy.Services.AUTH;
using Microsoft.EntityFrameworkCore;
using EasyBuy.Models.MOMO;
using EasyBuy.Services.MOMO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using EasyBuy.Services.SIMPLECHAT;

var builder = WebApplication.CreateBuilder(args);
// Đăng ký EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

//MOMO API
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();


//Đăng ký VnPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();

// Đăng ký PDF Service

// Đăng ký dịch vụ MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// Cấu hình DbContext kết nối SQL Server
builder.Services.AddDbContext<EasyBuyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình dịch vụ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(40); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true;                 // Ngăn JS truy cập cookie
    options.Cookie.IsEssential = true;              // Bắt buộc với EU cookie law
});

//Configuration login google account
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.CallbackPath = "/signin-google";
});
//Chat API
builder.Services.AddScoped<LearningService>();
builder.Services.AddScoped<SimpleChatService>();

//Authentication & Authorization
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Middleware xử lý lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Chuyển hướng đến trang lỗi tùy chỉnh
    app.UseHsts(); // Bảo vệ HTTPS
}

// Middleware tĩnh & định tuyến
app.UseHttpsRedirection();    // Chuyển sang HTTPS
app.UseStaticFiles();         // Cho phép truy cập wwwroot

app.UseRouting();             // Bắt đầu định tuyến
app.UseAuthentication();      // Kích hoạt xác thực
app.UseSession();             // Kích hoạt session
app.UseAuthorization();       // Kích hoạt phân quyền nếu có

// Cấu hình route cho sản phẩm với URL đẹp
app.MapControllerRoute(
    name: "productDetails",
    pattern: "product/{productId:int}",
    defaults: new { controller = "Home", action = "ViewProductDetails" });

app.MapControllerRoute(
    name: "productDetailsAlt",
    pattern: "Home/ViewProductDetails/{productId:int}",
    defaults: new { controller = "Home", action = "ViewProductDetails" });

// Cấu hình route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    
// Cấu hình route MVC mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TrangChu}/{id?}");


app.Run();