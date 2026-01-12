# 🎵 Music API Project

Müzik endüstrisi için geliştirilmiş kapsamlı bir **RESTful Web API** projesidir. Bu API ile sanatçılar, albümler, şarkılar, müzik şirketleri (label), konserler ve kullanıcı yönetimi işlemleri gerçekleştirilebilir.

**Geliştirici:** Diyar Andak  
**Framework:** .NET 9.0  
**Veritabanı:** SQLite (Entity Framework Core ile)

---

## 📋 İçindekiler

1. [Proje Hakkında](#-proje-hakkında)
2. [Mimari Yapı](#-mimari-yapı)
3. [Veritabanı Şeması ve Entity İlişkileri](#-veritabanı-şeması-ve-entity-i̇lişkileri)
4. [Kullanılan Teknolojiler ve Özellikler](#-kullanılan-teknolojiler-ve-özellikler)
5. [Proje Klasör Yapısı](#-proje-klasör-yapısı)
6. [API Endpoint Listesi](#-api-endpoint-listesi)
7. [API Response Örnekleri](#-api-response-örnekleri)
8. [Kurulum Talimatları](#-kurulum-talimatları)
9. [Varsayılan Kullanıcı Bilgileri](#-varsayılan-kullanıcı-bilgileri)

---

## 🎯 Proje Hakkında

Bu proje, müzik sektörüne yönelik bir API sistemi sunmaktadır. Kullanıcılar kayıt olabilir, giriş yapabilir ve JWT token ile yetkilendirme gerektiren işlemleri (ekleme, güncelleme, silme) gerçekleştirebilir.

### Temel Özellikler

| Özellik | Açıklama |
|---------|----------|
| **CRUD İşlemleri** | Tüm entity'ler için Create, Read, Update, Delete işlemleri |
| **JWT Authentication** | Güvenli token tabanlı kimlik doğrulama |
| **Soft Delete** | Veriler kalıcı olarak silinmez, `IsDeleted` flag'i ile işaretlenir |
| **Global Exception Handling** | Middleware ile merkezi hata yönetimi |
| **Serilog Logging** | Konsol ve dosya tabanlı loglama sistemi |
| **DTO Pattern** | Entity'ler doğrudan expose edilmez, DTO'lar kullanılır |
| **Dependency Injection** | Servisler interface tabanlı DI ile yönetilir |

---

## 🏗 Mimari Yapı

Proje **Katmanlı Mimari (Layered Architecture)** prensibine uygun olarak tasarlanmıştır:

```
┌─────────────────────────────────────────────────────────────────┐
│                        PRESENTATION LAYER                        │
│  ┌─────────────────────────┐    ┌─────────────────────────────┐ │
│  │   Controllers (MVC)     │    │   Minimal API Endpoints     │ │
│  │  • UserController       │    │  • /api/song/*              │ │
│  │  • ArtistController     │    │  • /api/concert/*           │ │
│  │  • AlbumController      │    │  • /api/label/*             │ │
│  └─────────────────────────┘    └─────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────┐
│                        BUSINESS LAYER                            │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │                      Services                                ││
│  │  IUserService / UserService                                  ││
│  │  IArtistService / ArtistService                              ││
│  │  IAlbumService / AlbumService                                ││
│  │  ISongService / SongService                                  ││
│  │  ILabelService / LabelService                                ││
│  │  IConcertService / ConcertService                            ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────┐
│                         DATA LAYER                               │
│  ┌───────────────────┐    ┌───────────────────────────────────┐ │
│  │   AppDbContext    │    │         Entities                  │ │
│  │   (EF Core)       │    │  User, Artist, Album, Song,       │ │
│  │                   │◄───│  Label, Concert, BaseEntity       │ │
│  └───────────────────┘    └───────────────────────────────────┘ │
│            │                                                     │
│            ▼                                                     │
│  ┌───────────────────┐                                          │
│  │   SQLite (app.db) │                                          │
│  └───────────────────┘                                          │
└─────────────────────────────────────────────────────────────────┘
```

### Middleware Pipeline

```
Request → GlobalExceptionMiddleware → Authentication → Authorization → Controllers/MinimalAPI → Response
```

---

## 🗄 Veritabanı Şeması ve Entity İlişkileri

### Entity Relationship Diagram (ERD)

```
┌──────────────┐         ┌──────────────┐         ┌──────────────┐
│    Label     │ 1     N │    Artist    │ 1     N │    Album     │
│──────────────│─────────│──────────────│─────────│──────────────│
│ Id (PK)      │         │ Id (PK)      │         │ Id (PK)      │
│ Name         │         │ Name         │         │ Name         │
│ Country      │         │ Bio          │         │ ReleaseDate  │
│ CreatedAt    │         │ LabelId (FK) │         │ Price        │
│ UpdatedAt    │         │ CreatedAt    │         │ ArtistId(FK) │
│ IsDeleted    │         │ UpdatedAt    │         │ CreatedAt    │
└──────────────┘         │ IsDeleted    │         │ UpdatedAt    │
                         │              │         │ IsDeleted    │
                         │              │         └──────┬───────┘
                         │              │                │ 1
                         │              │                │
                         │              │                │ N
                         │              │         ┌──────┴───────┐
                         │              │         │     Song     │
                         │              │         │──────────────│
                         │              │         │ Id (PK)      │
                         │              │         │ Name         │
                         │              │         │ DurationSecs │
                         └──────┬───────┘         │ TrackNumber  │
                                │                 │ AlbumId (FK) │
                                │ 1               │ CreatedAt    │
                                │                 │ UpdatedAt    │
                                │ N               │ IsDeleted    │
                         ┌──────┴───────┐         └──────────────┘
                         │   Concert    │
                         │──────────────│         ┌──────────────┐
                         │ Id (PK)      │         │     User     │
                         │ Venue        │         │──────────────│
                         │ City         │         │ Id (PK)      │
                         │ Date         │         │ FullName     │
                         │ ArtistId(FK) │         │ Email        │
                         │ CreatedAt    │         │ Password     │
                         │ UpdatedAt    │         │ Role         │
                         │ IsDeleted    │         │ CreatedAt    │
                         └──────────────┘         │ UpdatedAt    │
                                                  │ IsDeleted    │
                                                  └──────────────┘
```

### İlişki Tablosu

| İlişki | Tür | Açıklama |
|--------|-----|----------|
| Label → Artist | One-to-Many (1:N) | Bir müzik şirketi birden fazla sanatçıya sahip olabilir |
| Artist → Album | One-to-Many (1:N) | Bir sanatçı birden fazla albüm çıkarabilir |
| Artist → Concert | One-to-Many (1:N) | Bir sanatçı birden fazla konser verebilir |
| Album → Song | One-to-Many (1:N) | Bir albüm birden fazla şarkı içerebilir |

**Toplam:** 6 Entity, 4 İlişki

### BaseEntity (Ortak Alanlar)

Tüm entity'ler `BaseEntity` sınıfından türetilmiştir ve aşağıdaki ortak alanlara sahiptir:

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }              // Primary Key
    public DateTime CreatedAt { get; set; }  // Oluşturulma tarihi
    public DateTime? UpdatedAt { get; set; } // Güncellenme tarihi
    public bool IsDeleted { get; set; }      // Soft Delete flag
}
```

---

## 🛠 Kullanılan Teknolojiler ve Özellikler

### Framework ve Kütüphaneler

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| .NET | 9.0 | Ana framework |
| Entity Framework Core | 9.0 | ORM (Object-Relational Mapping) |
| SQLite | - | Veritabanı |
| JWT Bearer | - | Token tabanlı kimlik doğrulama |
| BCrypt.Net | - | Şifre hashleme |
| Serilog | - | Loglama |
| Swashbuckle (Swagger) | - | API dokümantasyonu |

### Uygulanan Özellikler

#### ✅ JWT Authentication
- Kullanıcılar `/api/user/login` endpoint'i ile token alır
- Token, `Authorization: Bearer {token}` header'ı ile gönderilir
- `[Authorize]` attribute'u ile korunan endpoint'ler sadece token ile erişilebilir

#### ✅ Soft Delete
- Veriler veritabanından kalıcı olarak silinmez
- `IsDeleted = true` olarak işaretlenir
- Global Query Filter ile silinen veriler otomatik olarak sorgudan hariç tutulur

```csharp
// AppDbContext.cs içinde
modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
```

#### ✅ Global Exception Handling
- `GlobalExceptionMiddleware` tüm hataları yakalar
- Hatalar loglanır ve istemciye tutarlı formatta döner
- Try-catch bloklarını tekrar tekrar yazmaya gerek kalmaz

#### ✅ Serilog Logging
- Konsola renkli log çıktısı
- `logs/log.txt` dosyasına günlük log kaydı
- Her istek ve hata otomatik loglanır

#### ✅ Standart API Response Format
Tüm endpoint'ler aynı response formatını kullanır:

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
}
```

#### ✅ DTO (Data Transfer Object) Pattern
- Entity'ler doğrudan API'ye expose edilmez
- Her işlem için ayrı DTO'lar kullanılır:
  - `CreateXxxDto` - Oluşturma işlemleri
  - `UpdateXxxDto` - Güncelleme işlemleri
  - `XxxResponseDto` - Okuma işlemleri

#### ✅ Dependency Injection
Tüm servisler interface tabanlı DI ile yönetilir:

```csharp
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
// ... diğer servisler
```

#### ✅ Controller ve Minimal API
Proje hem klasik Controller hem de Minimal API yaklaşımını kullanır:
- **Controllers:** User, Artist, Album
- **Minimal API:** Song, Concert, Label

---

## 📁 Proje Klasör Yapısı

```
MusicAPIProject/
├── Controllers/           # API Controller'ları
│   ├── AlbumController.cs
│   ├── ArtistController.cs
│   └── UserController.cs
│
├── Data/                  # Veritabanı katmanı
│   ├── AppDbContext.cs    # EF Core DbContext
│   └── DataSeeder.cs      # Başlangıç verileri
│
├── DTOs/                  # Data Transfer Objects
│   ├── AlbumDtos.cs
│   ├── ApiResponse.cs     # Standart response formatı
│   ├── ArtistDtos.cs
│   ├── ConcertDtos.cs
│   ├── LabelDtos.cs
│   ├── SongDtos.cs
│   └── UserDtos.cs
│
├── Entities/              # Veritabanı entity'leri
│   ├── Album.cs
│   ├── Artist.cs
│   ├── BaseEntity.cs      # Ortak base sınıf
│   ├── Concert.cs
│   ├── Label.cs
│   ├── Song.cs
│   └── User.cs
│
├── Middleware/            # Custom middleware'ler
│   └── GlobalExceptionMiddleware.cs
│
├── Migrations/            # EF Core migration dosyaları
│
├── Services/              # İş mantığı katmanı
│   ├── IAlbumService.cs   # Interface
│   ├── AlbumService.cs    # Implementation
│   ├── IArtistService.cs
│   ├── ArtistService.cs
│   ├── IConcertService.cs
│   ├── ConcertService.cs
│   ├── ILabelService.cs
│   ├── LabelService.cs
│   ├── ISongService.cs
│   ├── SongService.cs
│   ├── IUserService.cs
│   └── UserService.cs
│
├── logs/                  # Log dosyaları (Serilog)
├── Program.cs             # Uygulama giriş noktası
├── appsettings.json       # Konfigürasyon
└── app.db                 # SQLite veritabanı
```

---

## 🔌 API Endpoint Listesi

### 👤 User (Kullanıcı Yönetimi)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/user` | Tüm kullanıcıları listele | ❌ |
| POST | `/api/user/register` | Yeni kullanıcı kaydı | ❌ |
| POST | `/api/user/login` | Giriş yap, JWT token al | ❌ |
| PUT | `/api/user/{id}` | Kullanıcı güncelle | ✅ |
| DELETE | `/api/user/{id}` | Kullanıcı sil (soft delete) | ✅ Admin |

### 🎤 Artist (Sanatçı Yönetimi)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/artist` | Tüm sanatçıları listele | ❌ |
| GET | `/api/artist/{id}` | Sanatçı detayı | ❌ |
| POST | `/api/artist` | Yeni sanatçı ekle | ✅ |
| PUT | `/api/artist/{id}` | Sanatçı güncelle | ✅ |
| DELETE | `/api/artist/{id}` | Sanatçı sil | ✅ Admin |

### 💿 Album (Albüm Yönetimi)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/album` | Tüm albümleri listele | ❌ |
| GET | `/api/album/{id}` | Albüm detayı | ❌ |
| POST | `/api/album` | Yeni albüm ekle | ✅ |
| PUT | `/api/album/{id}` | Albüm güncelle | ✅ |
| DELETE | `/api/album/{id}` | Albüm sil | ✅ Admin |

### 🎵 Song (Şarkı Yönetimi - Minimal API)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/song` | Tüm şarkıları listele | ❌ |
| GET | `/api/song/{id}` | Şarkı detayı | ❌ |
| POST | `/api/song` | Yeni şarkı ekle | ✅ |
| PUT | `/api/song/{id}` | Şarkı güncelle | ✅ |
| DELETE | `/api/song/{id}` | Şarkı sil | ✅ Admin |

### 🎪 Concert (Konser Yönetimi - Minimal API)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/concert` | Tüm konserleri listele | ❌ |
| GET | `/api/concert/{id}` | Konser detayı | ❌ |
| POST | `/api/concert` | Yeni konser ekle | ✅ |
| PUT | `/api/concert/{id}` | Konser güncelle | ✅ |
| DELETE | `/api/concert/{id}` | Konser sil | ✅ Admin |

### 🏢 Label (Müzik Şirketi - Minimal API)
| HTTP | Endpoint | Açıklama | Auth |
|------|----------|----------|------|
| GET | `/api/label` | Tüm şirketleri listele | ❌ |
| GET | `/api/label/{id}` | Şirket detayı | ❌ |
| POST | `/api/label` | Yeni şirket ekle | ✅ |
| PUT | `/api/label/{id}` | Şirket güncelle | ✅ |
| DELETE | `/api/label/{id}` | Şirket sil | ✅ Admin |

**Toplam:** 30 Endpoint (6 Entity × 5 CRUD İşlemi)

---

## 📝 API Response Örnekleri

### ✅ Başarılı Listeleme (200 OK)

**GET** `/api/artist`

```json
{
  "success": true,
  "message": "Sanatçılar listelendi",
  "data": [
    {
      "id": 1,
      "name": "Tarkan",
      "bio": "Pop müziğin megastarı",
      "labelId": 1,
      "labelName": "DMC"
    },
    {
      "id": 2,
      "name": "Sezen Aksu",
      "bio": "Türk pop müziğinin minik serçesi",
      "labelId": 2,
      "labelName": "Sony Music"
    }
  ]
}
```

### ✅ Başarılı Oluşturma (201 Created)

**POST** `/api/album`

Request Body:
```json
{
  "name": "Karma",
  "releaseDate": "2001-01-01",
  "price": 49.99,
  "artistId": 1
}
```

Response:
```json
{
  "success": true,
  "message": "Albüm eklendi",
  "data": {
    "id": 5
  }
}
```

### ✅ Başarılı Giriş (JWT Token)

**POST** `/api/user/login`

Request Body:
```json
{
  "email": "admin@music.com",
  "password": "123"
}
```

Response:
```json
{
  "success": true,
  "message": "Giriş başarılı",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### ❌ Hatalı Giriş (401 Unauthorized)

**POST** `/api/user/login`

```json
{
  "success": false,
  "message": "E-posta veya şifre hatalı",
  "data": null
}
```

### ❌ Kaynak Bulunamadı (404 Not Found)

**GET** `/api/album/999`

```json
{
  "success": false,
  "message": "Albüm bulunamadı",
  "data": null
}
```

### ❌ Sunucu Hatası (500 Internal Server Error)

```json
{
  "success": false,
  "message": "Sunucu hatası: Detaylı hata mesajı burada görünür",
  "data": null
}
```

---

## 🚀 Kurulum Talimatları

### Gereksinimler

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Bir kod editörü (VS Code, Visual Studio, Rider vb.)

### Adım Adım Kurulum

**1. Projeyi İndirin**

```bash
git clone https://github.com/diyarandak/MusicAPIProjec.git
cd MusicAPIProjec
```

**2. Bağımlılıkları Yükleyin**

```bash
cd MusicAPIProject
dotnet restore
```

**3. Veritabanını Oluşturun**

> ⚠️ Bu adım isteğe bağlıdır. Uygulama ilk çalıştığında migration otomatik uygulanır.

```bash
dotnet ef database update
```

**4. Uygulamayı Çalıştırın**

```bash
dotnet run
```

**5. Swagger UI'a Erişin**

Uygulama başladığında konsolda görünen port numarasını kullanın:

```
http://localhost:{PORT}/swagger
```

> 💡 Port numarası genellikle `5000`, `5001`, `5202` gibi değerler olabilir. Konsol çıktısında "Now listening on: http://localhost:XXXX" satırına bakın.

### JWT Token Kullanımı (Swagger'da)

1. `/api/user/login` endpoint'ine POST isteği gönderin
2. Dönen `token` değerini kopyalayın
3. Swagger sayfasında sağ üstteki **"Authorize"** butonuna tıklayın
4. Token'ı `Bearer {token}` formatında girin (örn: `Bearer eyJhbGciOiJIUzI...`)
5. Artık yetkilendirme gerektiren endpoint'leri kullanabilirsiniz

---

## 👤 Varsayılan Kullanıcı Bilgileri

Uygulama ilk çalıştığında `DataSeeder` tarafından oluşturulan kullanıcılar:

| Rol | E-posta | Şifre |
|-----|---------|-------|
| Admin | admin@music.com | 123 |
| User | user@music.com | 123 |

> 🔐 Şifreler veritabanında **BCrypt** ile hashlenmiş olarak saklanır.

---

## 📊 Proje Özet Tablosu

| Metrik | Değer |
|--------|-------|
| Entity Sayısı | 6 (User, Artist, Album, Song, Label, Concert) |
| İlişki Sayısı | 4 (One-to-Many) |
| Toplam Endpoint | 30 |
| Controller Sayısı | 3 (User, Artist, Album) |
| Minimal API Grubu | 3 (Song, Concert, Label) |
| Service Sayısı | 6 |
| DTO Dosyası | 7 |

---

## 📜 Lisans

Bu proje eğitim amaçlı gelıştirilmiştir.
