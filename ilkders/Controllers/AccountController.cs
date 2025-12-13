using Site.Models;
using Site.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Site.Data;
using System.Linq;

namespace Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
        }

        // --- GİRİŞ YAP ---
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Lütfen tüm alanları doldurunuz.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded) return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "E-posta adresi veya şifre hatalı.";
            return View();
        }

        // --- KAYIT OL ---
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    KayitTarihi = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // --- ŞİFREMİ UNUTTUM ---
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) return View("ForgotPasswordConfirmation");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: Request.Scheme);

                string mesaj = $@"<div style='font-family: Arial; padding: 20px;'>
                                    <h2>Şifre Sıfırlama</h2>
                                    <p>Merhaba {user.Ad}, şifreni sıfırlamak için tıkla:</p>
                                    <a href='{callbackUrl}' style='background:#667eea; color:white; padding:10px 20px; text-decoration:none; border-radius:5px;'>Şifremi Sıfırla</a>
                                  </div>";
                try { await _emailService.SendEmailAsync(model.Email, "Şifre Sıfırlama", mesaj); } catch { }

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null) { ViewBag.Error = "Geçersiz istek."; return View("Login"); }
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation() => View();

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        // --- PROFİL VE İSTATİSTİKLER (DÜZELTİLDİ) ---
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (user.KayitTarihi.Year == 1)
            {
                user.KayitTarihi = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }

            // İstatistikleri Hesapla
            var kullaniciKitaplari = _context.Kitaplar.Where(k => k.UserId == user.Id).ToList();

            var model = new ProfileViewModel
            {
                Ad = user.Ad,
                Soyad = user.Soyad,
                Email = user.Email ?? "",
                Telefon = user.PhoneNumber,
                UyelikTarihi = user.KayitTarihi,

                KitapSayisi = kullaniciKitaplari.Count,
                OkunanKitapSayisi = kullaniciKitaplari.Count(k => k.Durum == "Okundu"),
                OkunuyorSayisi = kullaniciKitaplari.Count(k => k.Durum == "Okunuyor"),

                // HATA DÜZELTİLDİ: "?? 0" kaldırıldı çünkü SayfaSayısı int (boş olamaz)
                ToplamSayfa = kullaniciKitaplari
                                .Where(k => k.Durum == "Okundu" && k.SayfaSayısı > 0)
                                .Sum(k => k.SayfaSayısı),

                FavoriTur = kullaniciKitaplari
                                .GroupBy(k => k.Tür)
                                .OrderByDescending(g => g.Count())
                                .Select(g => g.Key)
                                .FirstOrDefault() ?? "-"
            };

            return View(model);
        }

        // --- PROFİL GÜNCELLEME ---
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            ModelState.Remove("Telefon");
            // ... Telefon validasyonu eklenebilir ...

            if (ModelState.IsValid)
            {
                user.Ad = model.Ad;
                user.Soyad = model.Soyad;
                user.PhoneNumber = model.Telefon;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) ViewBag.Message = "Profil güncellendi.";
                else ViewBag.Message = "Hata oluştu.";
            }

            // Sayfa yenilenince istatistikleri tekrar hesapla
            var kullaniciKitaplari = _context.Kitaplar.Where(k => k.UserId == user.Id).ToList();

            model.Email = user.Email ?? "";
            model.UyelikTarihi = user.KayitTarihi;
            model.KitapSayisi = kullaniciKitaplari.Count;
            model.OkunanKitapSayisi = kullaniciKitaplari.Count(k => k.Durum == "Okundu");
            model.OkunuyorSayisi = kullaniciKitaplari.Count(k => k.Durum == "Okunuyor");

            // BURADA DA DÜZELTME YAPILDI:
            model.ToplamSayfa = kullaniciKitaplari
                                .Where(k => k.Durum == "Okundu" && k.SayfaSayısı > 0)
                                .Sum(k => k.SayfaSayısı);

            model.FavoriTur = kullaniciKitaplari.GroupBy(k => k.Tür).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault() ?? "-";

            return View(model);
        }

        // --- HESAP SİLME ---
        [HttpGet]
        public IActionResult DeleteAccount() => View();

        [HttpPost]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var kullaniciKitaplari = _context.Kitaplar.Where(k => k.UserId == user.Id);
            _context.Kitaplar.RemoveRange(kullaniciKitaplari);

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Profile");
        }
    }
}