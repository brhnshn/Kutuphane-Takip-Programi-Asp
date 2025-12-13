// Sayfa yüklendiğinde çalışacak kodlar
document.addEventListener("DOMContentLoaded", () => {
    // Smooth scroll için tüm iç bağlantıları seç
    const links = document.querySelectorAll('a[href^="#"]');

    // Her bağlantıya tıklandığında smooth scroll uygula
    links.forEach((link) => {
        link.addEventListener("click", function (e) {
            e.preventDefault();

            const targetId = this.getAttribute("href");
            if (targetId === "#") return;

            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                const header = document.querySelector("header");
                const headerOffset = header ? header.offsetHeight : 80;
                window.scrollTo({
                    top: targetElement.offsetTop - headerOffset,
                    behavior: "smooth",
                });
            }
        });
    });

    // NOT: Form submit handling burada KALDIRILDI.
    // Formu JS ile gönderen/işleyen kod artık yalnızca main.js içinde bulunmalı.
    // Eğer yine de burada basit doğrulama vs. istersen, main.js'deki Ajax handler'ına engel olmayacak şekilde
    // sadece UI doğrulaması yapacak biçimde ekleyebilirim.

    // Sayfa scroll olduğunda header'a gölge ekle (header varsa)
    window.addEventListener("scroll", () => {
        const header = document.querySelector("header");
        if (!header) return;
        if (window.scrollY > 50) {
            header.style.boxShadow = "0 2px 10px rgba(0, 0, 0, 0.1)";
        } else {
            header.style.boxShadow = "none";
        }
    });
});

// Sayfa yüklendiğinde animasyon ekle
window.addEventListener("load", () => {
    // Tüm içerik kartlarını seç
    const cards = document.querySelectorAll(".card");

    // Her karta görünürlük sınıfı ekle (CSS'te tanımlanabilir)
    cards.forEach((card, index) => {
        setTimeout(() => {
            card.style.opacity = "1";
            card.style.transform = "translateY(0)";
        }, 200 * index);
    });
});

// Mobil menü için basit toggle (nav varsa)
function toggleMobileMenu() {
    const nav = document.querySelector("nav ul");
    if (!nav) return;
    nav.classList.toggle("show");
}
