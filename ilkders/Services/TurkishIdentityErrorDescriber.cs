using Microsoft.AspNetCore.Identity;

namespace Site // Projenin ana namespace'i
{
    // IdentityErrorDescriber sınıfından miras alıyoruz
    public class TurkishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                // İŞTE İSTEDİĞİN TÜRKÇE MESAJ:
                Description = "Şifreniz en az bir adet özel karakter (sembol) içermelidir. Örn: @, #, $, %, &"
            };
        }

        // BU KISIM BONUS: Diğer zorunlu kuralları da Türkçeleştirelim
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Şifreniz en az bir adet rakam (0-9) içermelidir."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Şifreniz en az bir adet küçük harf (a-z) içermelidir."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Şifreniz en az bir adet büyük harf (A-Z) içermelidir."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"'{email}' adresi zaten kullanılıyor."
            };
        }

        // ... (Kalan diğer metodları da ekleyebilirsin)
    }
}