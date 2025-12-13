using Microsoft.AspNetCore.Identity;
using Site.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Models
{
    public class Kitap
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        [Display(Name = "Kitap Adı")]
        [StringLength(50, ErrorMessage = "Kitap adı en fazla 50 karakter olabilir.")]
        public string KitapAdı { get; set; }

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [Display(Name = "Yazar Adı")]
        [StringLength(50, ErrorMessage = "Yazar adı en fazla 50 karakter olabilir.")]
        public string YazarAdı { get; set; }

        [Display(Name = "Yayın Evi")]
        [StringLength(50, ErrorMessage = "Yayın evi adı en fazla 50 karakter olabilir.")]
        public string YayınEvi { get; set; }

        [Display(Name = "Sayfa Sayısı")]
        [Range(1, 5000, ErrorMessage = "Sayfa sayısı 1 ile 5000 arasında olmalıdır.")]
        public int SayfaSayısı { get; set; }

        [Display(Name = "Basım Yılı")]
        [Range(1000, 9999, ErrorMessage = "Lütfen 4 haneli geçerli bir yıl giriniz.")]
        public int Yıl { get; set; }

        [Required(ErrorMessage = "Lütfen bir tür seçiniz.")]
        public string Tür { get; set; }

        [Display(Name = "ISBN No")]
        [StringLength(20, ErrorMessage = "ISBN numarası çok uzun.")]
        public string? ISBN { get; set; }

        [Required]
        public string Durum { get; set; } = "Listede";

        [Range(1, 5)]
        public int Puan { get; set; } = 0;
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

    }
}