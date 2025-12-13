using Site;
using Site.Data;
using Site.Models;
using Site.Services; // YENİ EKLENDİ: EmailService için
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity Ayarları
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Şifre Kurtarma Token'larının ömrünü ayarlayabiliriz (Örn: 2 saat)
    // options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<TurkishIdentityErrorDescriber>();

// 3. Çerez Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// --- MAİL SERVİSİ TANIMLAMASI (YENİ EKLENDİ) ---
builder.Services.AddTransient<IEmailService, EmailService>();
// ---------------------------------------------

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- HATA YÖNETİMİ ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
else
{
    // Geliştirme ortamında da hata sayfasını gör
    app.UseExceptionHandler("/Error/500");
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
// --------------------

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Kimlik Doğrulama
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AnaSayfa}/{action=Index}/{id?}"); // AnaSayfa yerine Home olarak düzelttim (Controller adına göre)

app.Run();