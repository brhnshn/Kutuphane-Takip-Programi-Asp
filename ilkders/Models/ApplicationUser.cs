using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Site.Models  // <-- Program.cs ve DbContext ile aynı soyadını taşıyor
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Ad")]
        public string? Ad { get; set; }

        [Display(Name = "Soyad")]
        public string? Soyad { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}