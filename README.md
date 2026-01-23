# 📚 Kütüphane Takip Yönetim Sistemi

> ASP.NET Core MVC ile geliştirilmiş, güvenli üyelik yapısına sahip, canlı ortamda yayınlanan web tabanlı kütüphane yönetim uygulaması.

---

## 🔎 Proje Özeti

**Kütüphane Takip Yönetim Sistemi**, kullanıcıların kişisel kitap koleksiyonlarını dijital ortamda güvenli ve düzenli biçimde yönetmesini sağlar. Modern ASP.NET Core mimarisi, Identity tabanlı kimlik doğrulama ve Entity Framework Core ile kalıcı veri depolama sunar. Uygulama production ortamına taşınmış ve aktif olarak çalışmaktadır.

---

## ✨ Özellikler

### 📘 Kitap Yönetimi (CRUD)

* Kitap ekleme, listeleme, güncelleme ve silme
* Alanlar: **Kitap Adı, Yazar, Yayın Evi, Tür, Yıl, Sayfa Sayısı, ISBN**

### 👤 Üyelik & Güvenlik

* ASP.NET Core Identity ile güvenli kayıt ve giriş
* Parola sıfırlama ve e‑posta doğrulama
* Yetkilendirme (kullanıcı bazlı işlem kontrolü)

### 🎨 Kullanıcı Deneyimi

* **Dark / Light tema** (anlık geçiş)
* Responsive ve sade arayüz

### 🗄️ Veri Yönetimi

* **SQL Server** ile kalıcı veri depolama
* **Entity Framework Core** (Code‑First, Migrations)

### 🚀 Yayın

* **IIS** üzerinde production ortamında yayın
* Ortam bazlı yapılandırma (Development / Production)

---

## 🛠️ Teknolojiler

| Katman        | Teknoloji                 |
| ------------- | ------------------------- |
| Backend       | **ASP.NET Core MVC (C#)** |
| ORM           | **Entity Framework Core** |
| Veritabanı    | **SQL Server**            |
| Kimlik        | **ASP.NET Core Identity** |
| Frontend      | **HTML, CSS, JavaScript** |
| Sunucu        | **IIS (Windows)**         |
| Sürüm Kontrol | **Git & GitHub**          |

---

## 📂 Proje Yapısı

```
├── Controllers
├── Models
├── ViewModels
├── Services
├── Views
│   ├── Account
│   ├── AnaSayfa
│   ├── Home
│   ├── Kitap
│   └── Shared
├── Data
├── Migrations
├── wwwroot
│   └── img
├── appsettings.json
├── Program.cs
└── README.md
```

---

## 🚀 Kurulum

Yerel ortamda çalıştırmak için:

1. Depoyu klonlayın

   ```bash
   git clone https://github.com/kullaniciadi/repo-adi.git
   ```
2. Visual Studio’da projeyi açın
3. `appsettings.json` içinde **SQL Server Connection String** bilgisini güncelleyin
4. Migration’ları uygulayın

   ```bash
   Update-Database
   ```
5. Uygulamayı çalıştırın (F5)

---

## 🔐 Güvenlik Notları

* Gizli anahtarlar ve bağlantı dizeleri repoya eklenmemelidir
* Production ortamında **HTTPS** zorunlu tutulmalıdır
* Identity parola politikaları aktiftir

---

## 🧪 Geliştirme Notları

* MVC katmanları ayrıştırılmıştır
* Code‑First yaklaşımı kullanılmıştır
* Genişletilebilir mimari (rol bazlı yetkilendirme eklenebilir)

---

## 📸 Ekran Görüntüleri

> `img/` klasörüne ekran görüntüleri ekleyebilirsiniz.

---

## 📬 İletişim

Geri bildirim, öneri veya iş birliği için:

* **E‑posta:** [sahinburhan501@gmail.com](mailto:sahinburhan501@gmail.com)
* **GitHub:** [https://github.com/brhnshn](https://github.com/brhnshn)
* **Linkedin:** [https://www.linkedin.com/in/burhan-sahin/](https://www.linkedin.com/in/burhan-sahin/)

---

## 📄 Lisans

Bu proje **MIT License** ile lisanslanmıştır.
