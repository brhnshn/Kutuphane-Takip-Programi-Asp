using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Site.Models;

namespace Site.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // --- RASTGELE İPUCU SİSTEMİ ---

        var ipuclari = new List<string>
        {
            "Kitap eklerken ISBN numarasını girersen veriler daha düzenli olur.",
            "Arama çubuğunu kullanarak yüzlerce kitap arasından istediğini saniyeler içinde bulabilirsin.",
            "Profil sayfasından kütüphanendeki toplam kitap sayısını ve üyelik tarihini görebilirsin.",
            "Kitaplarını türlerine göre (Roman, Tarih vb.) kategorize etmek aradığını bulmanı kolaylaştırır.",
            "Yanlış girdiğin bir kitabı silmek yerine 'Düzenle' butonunu kullanabilirsin.",
            "Kütüphanen tamamen sana özeldir (izole veritabanı), başkaları senin listeni göremez.",
            "Basım yılı bilgisini girmek, kütüphanenin kronolojik düzeni için önemlidir.",
            "Sayfa sayılarını doğru girmek, okuma hedeflerini takip etmene yardımcı olur."
        };

        var random = new Random();
        int index = random.Next(ipuclari.Count);

        // Seçilen cümleyi View'a taşıyoruz
        ViewBag.Ipucu = ipuclari[index];

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}