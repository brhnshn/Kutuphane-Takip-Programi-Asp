using Site.Models;
using Site.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Site.Controllers
{
    [Authorize]
    public class KitapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KitapController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // LİSTELEME (GÜNCELLENDİ: FİLTRELEME EKLENDİ)
        public async Task<IActionResult> Index(string aramaMetni, string durumFiltresi, int? pageNumber)
        {
            var userId = GetCurrentUserId();

            // Arama ve Filtre bilgilerini View'a taşı (Sayfalama ve buton boyama için)
            ViewData["CurrentFilter"] = aramaMetni;
            ViewBag.AktifFiltre = durumFiltresi;

            var kitaplar = from k in _context.Kitaplar
                           where k.UserId == userId
                           select k;

            // 1. ARAMA YAPILDIYSA
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                kitaplar = kitaplar.Where(s => s.KitapAdı.Contains(aramaMetni)
                                            || s.YazarAdı.Contains(aramaMetni)
                                            || s.Tür.Contains(aramaMetni));
            }

            // 2. FİLTRE SEÇİLDİYSE (Okundu, Okunuyor vb.)
            if (!string.IsNullOrEmpty(durumFiltresi))
            {
                kitaplar = kitaplar.Where(k => k.Durum == durumFiltresi);
            }

            kitaplar = kitaplar.OrderByDescending(k => k.ID);
            int pageSize = 8;

            return View(await PaginatedList<Kitap>.CreateAsync(kitaplar.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // EKLEME (GET)
        public IActionResult Create()
        {
            ViewBag.Turler = GetTurListesi();
            ViewBag.Durumlar = GetDurumListesi(); // Eklendi
            return View();
        }

        // EKLEME (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kitap kitap)
        {
            kitap.UserId = GetCurrentUserId();

            // --- YENİ: TARİHİ ŞİMDİ OLARAK AYARLA ---
            kitap.EklenmeTarihi = DateTime.Now;
            // ----------------------------------------

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Add(kitap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ... Hata kodları aynı kalsın ...
            ViewBag.Turler = GetTurListesi();
            ViewBag.Durumlar = GetDurumListesi();
            return View(kitap);
        }

        // DÜZENLEME (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var kitap = await _context.Kitaplar.FirstOrDefaultAsync(m => m.ID == id && m.UserId == userId);

            if (kitap == null) return NotFound();

            // Dropdownlar için listeleri gönder
            ViewBag.Turler = GetTurListesi();
            ViewBag.Durumlar = GetDurumListesi(); // DÜZELTME: Buraya eklendi

            return View(kitap);
        }

        // DÜZENLEME (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kitap kitap)
        {
            if (id != kitap.ID) return NotFound();

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutKitap = await _context.Kitaplar.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

                    if (mevcutKitap != null)
                    {
                        kitap.UserId = mevcutKitap.UserId;
                        _context.Update(kitap);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Details), new { id = kitap.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KitapExists(kitap.ID)) return NotFound();
                    else throw;
                }
            }

            // Hata varsa listeleri tekrar yükle
            ViewBag.Turler = GetTurListesi();
            ViewBag.Durumlar = GetDurumListesi(); // DÜZELTME: Buraya eklendi
            return View(kitap);
        }

        // DETAY (GET)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var kitap = await _context.Kitaplar
                .FirstOrDefaultAsync(m => m.ID == id && m.UserId == userId);

            if (kitap == null) return NotFound();

            return View(kitap);
        }

        // SİLME (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var kitap = await _context.Kitaplar.FirstOrDefaultAsync(m => m.ID == id && m.UserId == userId);

            if (kitap == null) return NotFound();

            return View(kitap);
        }

        // SİLME (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var kitap = await _context.Kitaplar.FirstOrDefaultAsync(x => x.ID == id && x.UserId == userId);
            if (kitap != null)
            {
                _context.Kitaplar.Remove(kitap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KitapExists(int id)
        {
            return _context.Kitaplar.Any(e => e.ID == id);
        }

        // YARDIMCI METODLAR
        private List<string> GetTurListesi()
        {
            return new List<string>() {
                "Roman", "Hikaye", "Bilim Kurgu", "Distopya",
                "Fantastik", "Masal",
                "Tarih", "Kişisel Gelişim", "Felsefe", "Şiir",
                "Biyografi", "Eğitim", "Diğer"
            };
        }

        private List<string> GetDurumListesi()
        {
            return new List<string>() {
                "Listede",      // Henüz başlanmadı (Varsayılan)
                "Okunuyor",     // Şu an okunuyor
                "Okundu",       // Bitti
                "Yarım Kaldı"   // Beğenmedim, bıraktım
            };
        }
    }
}