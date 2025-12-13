using System.ComponentModel.DataAnnotations;

namespace Site.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(20, ErrorMessage = "Ad en fazla 20 karakter olabilir.")]
        public string? Ad { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(20, ErrorMessage = "Soyad en fazla 20 karakter olabilir.")]
        public string? Soyad { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Phone(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz.")]
        public string? Telefon { get; set; }

        [Display(Name = "Email Adresi")]
        public string? Email { get; set; }

        // --- MEVCUT ALANLAR ---
        public int KitapSayisi { get; set; }
        public DateTime UyelikTarihi { get; set; }

        // --- YENİ EKLENENLER (İstatistik Dashboard İçin) ---
        public int OkunanKitapSayisi { get; set; }  // Bitenler
        public int OkunuyorSayisi { get; set; }     // Şu an okunanlar
        public int ToplamSayfa { get; set; }        // Toplam okunan sayfa
        public string? FavoriTur { get; set; }      // En çok sevilen tür
    }
}