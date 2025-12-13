// Sayfa tamamen yüklendiğinde çalış
document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById("themeToggle");
    const themeIcon = document.getElementById("themeIcon");
    const htmlElement = document.documentElement;

    // 1. Tarayıcı hafızasından (LocalStorage) temayı oku, yoksa 'light' varsay
    const currentTheme = localStorage.getItem("theme") || "dark";

    // 2. Sayfa açılır açılmaz o temayı uygula
    applyTheme(currentTheme);

    // 3. Butona tıklanınca ne olsun?
    if (themeToggleBtn) {
        themeToggleBtn.addEventListener("click", function () {
            // Şu anki tema ne?
            const currentSetting = htmlElement.getAttribute("data-bs-theme");
            // Eğer koyuysa açık yap, açıksa koyu yap
            const newTheme = currentSetting === "dark" ? "light" : "dark";

            // Yeni temayı uygula
            applyTheme(newTheme);
        });
    }

    // --- TEMAYI UYGULAMA FONKSİYONU ---
    function applyTheme(theme) {
        // HTML etiketine 'data-bs-theme="dark"' veya "light" yazar
        htmlElement.setAttribute("data-bs-theme", theme);

        // Seçimi hafızaya kaydet (Sayfa yenilenince gitmesin)
        localStorage.setItem("theme", theme);

        // İkonu güncelle
        updateIcon(theme);
    }

    // --- İKONU GÜNCELLEME FONKSİYONU ---
    function updateIcon(theme) {
        if (!themeIcon) return;

        // Önceki tüm sınıfları temizle (Sadece 'fas' kalsın)
        themeIcon.className = "fas";

        if (theme === "dark") {
            // KARANLIK MOD: Beyaz Ay İkonu
            themeIcon.classList.add("fa-moon");
            themeIcon.classList.add("text-white");
        } else {
            // AYDINLIK MOD: Siyah Güneş İkonu
            themeIcon.classList.add("fa-sun");
            themeIcon.classList.add("text-dark");
        }
    }
});