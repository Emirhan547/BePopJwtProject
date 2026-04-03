# 🎧 BePopJwtProject

**BePopJwtProject**, modern **Best Practices** ve **Clean Code prensipleri** esas alınarak geliştirilmiş; JWT tabanlı kimlik doğrulama, paket bazlı içerik erişimi ve **ML.NET destekli kişiselleştirilmiş müzik önerileri** sunan çok katmanlı bir **.NET müzik platformudur**.

Proje; sürdürülebilirlik, okunabilirlik ve genişletilebilirlik hedefleri doğrultusunda tasarlanmış olup, backend API ve MVC tabanlı web arayüzünden oluşur.

---

## 🚀 Özellikler

* 🔐 JWT ile güvenli kimlik doğrulama (register / login / profile)
* 🎵 Paket bazlı müzik erişimi (Free / Premium)
* 🧠 **ML.NET ile kişiselleştirilmiş öneri sistemi**
* 🎯 Mood & prompt bazlı akıllı öneriler (OpenAI destekli)
* ☁️ AWS S3 entegrasyonu (şarkı & görsel yükleme)
* 📊 Kullanıcı dinleme geçmişi takibi
* 🧱 Katmanlı mimari (Clean Architecture yaklaşımı)
* 📄 Scalar ile API dokümantasyonu

---

## 🧱 Mimari Yapı

```text
Entity → DataAccess → Business → API → WebUI
```

### Katmanlar

* **BePopJwt.Entity**
  Entity modelleri ve enum’lar

* **BePopJwt.DataAccess**
  EF Core, Repository, Unit of Work, Interceptor’lar

* **BePopJwt.Business**
  DTO’lar, servisler, validasyonlar, öneri algoritmaları (ML.NET dahil)

* **BePopJwt.API**
  Controller’lar, JWT/Identity, Options pattern config yönetimi

* **BePopJwt.WebUI**
  MVC UI ve API tüketimi

---

## 🧩 Kullanılan Design Pattern’ler

### ✅ Result Pattern

* Operasyon sonuçlarını standart hale getirir
* Success / Failure yapısı ile response yönetimi sağlar
* Controller katmanını sadeleştirir

```csharp
return Result.Success(data);
return Result.Failure("Bir hata oluştu");
```

---

### ⚙️ Options Pattern

* Strongly-typed configuration yönetimi
* `appsettings.json` → class mapping
* Dependency Injection ile güvenli erişim

---

### 🔁 Unit of Work (UOW)

* Tüm veritabanı işlemlerini tek transaction altında toplar
* Repository’ler arası koordinasyonu sağlar
* Veri tutarlılığını garanti eder

---

### 🧠 Interceptor Kullanımı

* EF Core interceptor’ları ile cross-cutting concern yönetimi
* Audit, logging, otomatik timestamp işlemleri
* Business logic’in sade ve temiz kalmasını sağlar

---

## 🤖 ML.NET ile Öneri Sistemi

Projede öneri mekanizması için **ML.NET** kullanılmıştır.

### Yaklaşım

* **Matrix Factorization (Collaborative Filtering)**
* Kullanıcı–şarkı etkileşim matrisi üzerinden öğrenme

### Akış

1. Kullanıcı dinleme geçmişi toplanır
2. Interaction dataset oluşturulur
3. Model eğitilir
4. Prediction ile öneri listesi üretilir

### Fallback

* Popüler içerikler
* Benzer kullanıcı davranışları
* Mood / keyword bazlı öneriler

---

## 🎯 Mood & Prompt Bazlı Öneriler

* OpenAI ile anahtar kelime üretimi
* ML.NET ile hibrit öneri sistemi
* API key yoksa local fallback mekanizması

---

## 📡 API Uçları

### 🔐 Auth

* `POST /api/Auths/register`
* `POST /api/Auths/login`
* `GET /api/Auths/profile`
* `PUT /api/Auths/profile`
* `POST /api/Auths/change-package`

---

### 🎧 Player

* `GET /api/Player/songs`
* `GET /api/Player/recommendations`
* `GET /api/Player/recommendations-by-mood`
* `GET /api/Player/recommendations-by-prompt`
* `POST /api/Player/play`
* `GET /api/Player/history`
* `GET /api/Player/song-source/{id}`

---

### 🗂️ İçerik Yönetimi

* `Albums`
* `Artists`
* `Songs`
* `Packages`
* `Banners`
* `UserSongHistories`

---

🖼️ Uygulama Görselleri

<img width="1920" height="3340" alt="screencapture-localhost-7044-Discovery-Discover-2026-04-03-21_07_59" src="https://github.com/user-attachments/assets/2082343b-9a63-4592-a5b4-188f2be12650" />

<img width="1920" height="1053" alt="screencapture-localhost-7044-Discovery-SongDetail-5-2026-04-03-21_08_51" src="https://github.com/user-attachments/assets/ac7fd8a0-e43c-473a-8e7d-fee3f4396fa3" />

<img width="1920" height="1327" alt="screencapture-localhost-7044-Artist-Artists-2026-04-03-21_10_03" src="https://github.com/user-attachments/assets/5ead020e-8338-45a5-b044-00be6841980b" />

<img width="1895" height="758" alt="Ekran görüntüsü 2026-04-03 211051" src="https://github.com/user-attachments/assets/73851a3c-36c7-4524-8d45-250a2c3d86e2" />

<img width="1920" height="3340" alt="screencapture-localhost-7044-Discovery-Discover-2026-04-03-21_07_59" src="https://github.com/user-attachments/assets/69b2a0e1-77b2-4216-a923-2f0ddb4e9f5a" />

