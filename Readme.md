<div align="center">
  <img src="https://img.icons8.com/color/96/000000/charity.png" alt="Logo"/>
  <h1>STK Bağış ve Yardım Yönetim Sistemi</h1>
  <p>
    Sivil Toplum Kuruluşları (STK) için geliştirilmiş; bağış toplama, yardım taleplerini değerlendirme, envanter yönetimi ve saha dağıtım süreçlerini uçtan uca otomatize eden kurumsal bir otomasyon sistemidir.
  </p>
</div>

<hr/>

## 🎯 Projenin Amacı ve Özeti
Bu proje, STK'ların günlük operasyonlarını merkezileştirmek, şeffaflığı artırmak ve iş akışını dijitalleştirmek amacıyla geliştirilmiştir. **N-Tier (Çok Katmanlı) Mimari** kullanılarak tasarlanmış olan bu sistem; gelişmiş rol tabanlı yetkilendirme (RBAC), yönetici onay mekanizmaları (Approval Workflow) ve canlı bildirim (Notification) altyapılarına sahiptir. 

Platform, bağışçıların güvenle sisteme katılıp finansal katkıda bulunmalarını sağlarken, saha görevlilerinin ve depo personellerinin ihtiyaç sahiplerine en hızlı şekilde destek ulaştırmasını kolaylaştırır. Üstelik finans ve envanter verileri role göre korunur ve yetkisiz erişimlere kapatılır.

## ✨ Temel Özellikler

1. 🔐 **Gelişmiş Rol ve Yetki Yönetimi (RBAC)**
   - `Admin`, `Muhasebeci (Accountant)`, `Saha Görevlisi (Worker)`, `Depo Sorumlusu (Staff)` ve `Bağışçı (Donor)` rolleri.
   - Hassas finansal verilere (Toplam Fon, Gider vb.) sadece yetkili personellerin erişmesini sağlayan korumalı Dashboard.

2. ✅ **Merkezi Onay Akışı (Approval Workflow)**
   - Yeni kullanıcı kayıtları, yapılan her türlü bağış ve yeni yardım talepleri güvenlik gereği doğrudan işleme alınmaz. Sistem yöneticisi (Admin) onayından geçer.
   - Onay veya ret durumunda ilgili kişiye (talep eden çalışan veya bağışçı) sistem içi otomatik bildirim düşer.

3. 🔔 **Bildirimler**
   - Yardım talepleri onaylandığında saha görevlisine, yeni kullanıcı kayıt olduğunda yöneticiye, bağış reddedildiğinde veya kabul edildiğinde bağışçıya bildirim gider.

4. 📦 **Envanter ve Stok Yönetimi**
   - Depodaki ayni bağışların (gıdadan giyime) miktar takibi.
   - Manuel stok ekleme, In/Out (Giriş/Çıkış) logları tutan `InventoryTransaction` hareket dökümleri.

5. 🚚 **Saha Görev Dağıtımı (Aid Distribution)**
   - Admin tarafından onaylanmış yardım talepleri, sahada çalışan bir "Field Worker"a atanır.
   - Çalışan kendisine atanan talebi inceler, teslimatı yapar ve sisteme "Teslim Edildi" logu düşerek bağlı bulunduğu depodan stoğu düşürür.

6. 💰 **Finansal Raporlama ve Kasa Durumu**
   - Tüm onaylı `Donation` (Gelir) ve kesilmiş `Expense` (Gider) tabloları canlı hesaplanarak, şık bir Glassmorphism "Bütçe Doluluk Oranı (Progress Bar)" ile gösterilir.
   
7. 🎨 **Premium UI/UX Tasarımı**
   - Modern, animasyonlu, cam efektli (Glassmorphism) Yönetim Paneli ve form giriş ekranları.

## 🛠️ Kullanılan Teknolojiler
- **Backend:** C#, ASP.NET Core MVC (.NET 10.0)
- **Mimari:** N-Tier Architecture (Repository & Service Pattern)
- **Veritabanı:** PostgreSQL / Entity Framework Core (Code-First - Npgsql)
- **Frontend:** HTML5, Vanilla CSS, Bootstrap 5, FontAwesome (Premium Dashboard UI)
- **Kimlik Doğrulama:** Cookie-based Authentication & Password Context Hashing (Identity standartlarına uyumlu özel altyapı)

## 🚀 Kurulum ve Çalıştırma (Kendi Bilgisayarında Derleme)

1. **Projeyi Klonlayın:**
   ```bash
   git clone https://github.com/melihkocc/Melih_Koc_NGODonationProject_SENG_321.git
   cd NgoDonationSystem
   ```

2. **Veritabanı Bağlantısını (ConnectionString) Ayarlayın:**
   - Proje dizinindeki `appsettings.json` dosyasını açın.
   - `DefaultConnection` string değerini kendi PostgreSQL kurulumunuza uygun şekilde düzenleyin.

3. **Veritabanını Oluşturun (Migrations):**
   ```bash
   dotnet ef database update
   ```
   *(Bu komut, kodda tanımlı modelleri okuyup `ApplicationDbContext` aracılığıyla veritabanı tablolarını eksiksiz olarak oluşturacaktır.)*

4. **Projeyi Başlatın:**
   ```bash
   dotnet run
   ```
   **veya** Visual Studio ortamında açıp <kbd>F5</kbd> tuşuyla projeyi çalıştırın.

## 🔑 Kullanıcı Akışı ve Test Senaryoları
Sistemi tam anlamıyla test edebilmek için aşağıdaki rollere göre hesaplar açabilirsiniz:
- Yeni bir hesap oluşturun (`Bağışçı`, `Saha Görevlisi` vb. seçerek).
- O oluşturduğunuz hesaba başlangıçta giremezsiniz, "Admin Onayı Bekliyor" hatası alırsınız.
- Projeyi ayağa kaldırdığınızda DB seviyesinden veya var olan Admin hesabından yetki vererek (Approval Action) yeni kullanıcının girişini açabilirsiniz.
- **Saha Görevlisi (Worker)** olarak girdiğinizde, diğer bağışları göremeyeceğinizi; **Admin** olarak girdiğinizde ise tam yetki listelerine sahip olduğunuzu gözlemleyebilirsiniz.

## 📄 Lisans
Bu proje geliştirme aşamasındadır ve örnek bir STK Yönetim Otomasyonu (Portföy ve Gerçek Vaka çalışması) niteliği taşımaktadır. Tüm hakları saklıdır.
