# 🎬 MovieWeb - Backend API

## 📋 Giới thiệu

MovieWebApp là hệ thống backend API cho ứng dụng xem phim trực tuyến, được xây dựng bằng **ASP.NET Core 9.0** với kiến trúc **Clean Architecture**. API cung cấp đầy đủ chức năng quản lý phim, người dùng, đánh giá, bình luận và yêu thích.

## 🏗️ Kiến trúc dự án

Dự án sử dụng **Clean Architecture** với 4 layers:

```
MovieWebApp/
├── Domain/              # Entities và Repository Interfaces
│   ├── Entities/        # Movie, User, Genre, Rating, Comment, Favorite
│   └── Repositories/    # IMovieRepository, IUserRepository, etc.
├── Application/         # Business Logic và DTOs
│   ├── Services/        # MovieService, AuthService, RatingService, etc.
│   ├── Interfaces/      # IMovieService, IAuthService, etc.
│   └── DTOs/           # Data Transfer Objects
├── Infrastructure/      # Database và Data Access
│   ├── Data/           # ApplicationDbContext
│   └── Repositories/   # Repository Implementations
└── Presentation/        # Controllers và API Endpoints
    └── Controllers/
        ├── Admin/      # Admin Controllers
        └── Client/     # Client Controllers
```

## 🚀 Công nghệ sử dụng

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 9.0
- **Authentication**: JWT Bearer Token
- **Cloud Storage**: Cloudinary (lưu trữ poster phim)
- **API Documentation**: Swagger/OpenAPI
- **Architecture Pattern**: Clean Architecture, Repository Pattern, Dependency Injection

## ⚙️ Cài đặt và chạy dự án

### Yêu cầu hệ thống:
- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt:

1. **Clone repository:**
   ```bash
   git clone <repository-url>
   cd MovieWebApp
   ```

2. **Cấu hình Database:**
   
   Mở file `appsettings.json` và cập nhật connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=MovieAPI;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Cấu hình JWT:**
   ```json
   "Jwt": {
     "Key": "YourSuperSecretKeyHereAtLeast32CharsLong",
     "Issuer": "MovieWebApp",
     "Audience": "MovieWebApp"
   }
   ```

4. **Cấu hình Cloudinary:**
   ```json
   "CloudinarySettings": {
     "CloudName": "your-cloud-name",
     "ApiKey": "your-api-key",
     "ApiSecret": "your-api-secret"
   }
   ```

5. **Chạy Migration:**
   ```bash
   dotnet ef database update
   ```

6. **Chạy ứng dụng:**
   ```bash
   dotnet run
   ```

7. **Truy cập Swagger UI:**
   ```
   https://localhost:7167
   ```

## 📊 Database Schema

### Entities chính:

- **Movie**: Phim (MovieId, MovieName, Description, ReleaseYear, Country, Poster, VideoUrl, Rating, ViewCount)
- **User**: Người dùng (UserId, UserName, Email, PasswordHash, Role)
- **Genre**: Thể loại phim (GenreId, GenreName)
- **Rating**: Đánh giá phim (RatingId, MovieId, UserId, StarRating, Review)
- **Comment**: Bình luận (CommentId, MovieId, UserId, Content)
- **Favorite**: Phim yêu thích (FavoriteId, MovieId, UserId)

## 🔐 Authentication & Authorization

### JWT Token Authentication
- Header: `Authorization: Bearer {token}`
- Token expiration: Cấu hình trong `appsettings.json`

### Roles:
- **Admin**: Quản lý toàn bộ hệ thống
- **User**: Người dùng thông thường

## 📚 API Endpoints

### 🔑 Authentication (`/api/Auth`)
- `POST /register` - Đăng ký tài khoản
- `POST /login` - Đăng nhập
- `GET /me` - Lấy thông tin user hiện tại

### 🎬 Movies (`/api/movies`)
- `GET /` - Lấy tất cả phim
- `GET /paged?pageNumber={n}&pageSize={s}&search={keyword}&genre={id}` - Lấy phim phân trang với search/filter
- `GET /{id}` - Lấy chi tiết phim

### 👑 Admin - Movies (`/api/admin/movies`)
- `POST /` - Tạo phim mới (FormData: poster file upload)
- `PUT /{id}` - Cập nhật phim (FormData: poster file upload)
- `DELETE /{id}` - Xóa phim (soft delete)
- `PATCH /{id}/toggle-status` - Bật/tắt trạng thái phim
- `GET /all?includeDeleted={true/false}` - Lấy tất cả phim (bao gồm đã xóa)

### 🎭 Genres (`/api/genres`)
- `GET /` - Lấy tất cả thể loại

### 👑 Admin - Genres (`/api/admin/genres`)
- `POST /` - Tạo thể loại mới
- `DELETE /{id}` - Xóa thể loại

### ⭐ Ratings (`/api/ratings`)
- `POST /` - Tạo đánh giá (StarRating, Review, MovieId)
- `PUT /{id}` - Cập nhật đánh giá
- `DELETE /{id}` - Xóa đánh giá
- `GET /movie/{movieId}` - Lấy tất cả đánh giá của phim
- `GET /movie/{movieId}/user` - Lấy đánh giá của user cho phim
- `GET /movie/{movieId}/average` - Lấy điểm trung bình của phim

### 💬 Comments (`/api/comments`)
- `POST /` - Tạo bình luận (Content, MovieId)
- `PUT /{id}` - Cập nhật bình luận
- `DELETE /{id}` - Xóa bình luận
- `GET /movie/{movieId}` - Lấy tất cả bình luận của phim
- `GET /user/{userId}` - Lấy tất cả bình luận của user

### ❤️ Favorites (`/api/favorites`)
- `GET /` - Lấy danh sách phim yêu thích của user hiện tại
- `POST /` - Thêm phim vào yêu thích (MovieId)
- `DELETE /{movieId}` - Xóa phim khỏi yêu thích

### 📊 Admin - Dashboard (`/api/Dashboard`)
- `GET /stats` - Thống kê tổng quan (Admin only)
  - Tổng users, movies, ratings, comments
  - Top 5 phim xem nhiều nhất
  - Top 5 phim đánh giá cao nhất

### 👥 Admin - Users (`/api/User`)
- `GET /` - Lấy tất cả người dùng (Admin only)
- `GET /{id}` - Lấy chi tiết user (Admin only)
- `PUT /{id}/role` - Cập nhật role user (Admin only)
  - Body: `{ "Role": "Admin" | "User" }`
- `DELETE /{id}` - Xóa user (Admin only)

### 👑 Admin - Ratings Management (`/api/Rating`)
- `GET /` - Lấy tất cả đánh giá (Admin only)
  - Bao gồm: userName, movieName, starRating, review, createdAt
- `DELETE /{id}` - Xóa đánh giá (Admin only)

### 👑 Admin - Comments Management (`/api/Comment`)
- `GET /` - Lấy tất cả bình luận (Admin only)
  - Bao gồm: userName, movieName, content, createdAt
- `DELETE /{id}` - Xóa bình luận (Admin only)

## 🔧 Các Service chính

### Application Layer Services

#### MovieService (IMovieService)
- Quản lý CRUD phim
- Tìm kiếm và phân trang
- Upload/Update poster qua Cloudinary
- Soft delete movies

#### AuthService (IAuthService)
- Đăng ký user mới
- Xác thực login (username/password)
- Tạo JWT token với role claims
- Password hashing với BCrypt

#### AdminService (IAdminService)
- Lấy thống kê dashboard (users, movies, ratings, comments count)
- Top 5 phim xem nhiều nhất / đánh giá cao nhất
- Quản lý users: CRUD, update role
- Quản lý ratings/comments: Get all, Delete

#### RatingService (IRatingService)
- Tạo/Cập nhật/Xóa đánh giá
- Lấy đánh giá theo phim/user
- Tính điểm trung bình phim
- Kiểm tra user đã đánh giá chưa

#### CommentService (ICommentService)
- Tạo/Cập nhật/Xóa bình luận
- Lấy bình luận theo phim/user
- Kiểm tra ownership (user chỉ sửa/xóa comment của mình)

#### FavoriteService (IFavoriteService)
- Thêm/Xóa phim yêu thích
- Lấy danh sách favorite của user
- Kiểm tra phim đã favorite chưa

#### GenreService (IGenreService)
- Quản lý thể loại phim
- CRUD genres

#### CloudinaryService
- Upload poster lên Cloudinary
- Delete poster từ Cloudinary
- Trả về URL poster

## 🛡️ Security Features

### Authentication & Authorization
- **JWT Bearer Token**: Xác thực với token trong header `Authorization: Bearer {token}`
- **Role-based Authorization**: 
  - `[Authorize(Roles = "Admin")]` - Chỉ Admin
  - `[Authorize(Roles = "User")]` - User thông thường
- **Password Hashing**: BCrypt để mã hóa mật khẩu
- **Token Claims**: UserId, Username, Role được embed trong JWT

### CORS Configuration
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Cho phép credentials
    });
});
```

### Data Protection
- **Soft Delete**: Movies có trường `IsDeleted` thay vì xóa vật lý
- **Owner Validation**: User chỉ sửa/xóa comment/rating của mình
- **Input Validation**: DTOs với Data Annotations

## 📝 Migrations

Dự án đã có các migrations:
- `Initial` - Tạo database ban đầu
- `IsDelate` - Thêm soft delete
- `UserMovie` - Quan hệ User-Movie
- `User` - Cập nhật User entity
- `IntialRole` - Thêm role system
- `AddRatingAndCommentFeatures` - Thêm Rating và Comment
- `FMovie` - Cập nhật Movie entity

## 🧪 Testing

Sử dụng Swagger UI để test API:
```
https://localhost:7167
```

## 📦 Dependencies

Các package NuGet chính:
- `Microsoft.EntityFrameworkCore.SqlServer` (9.0.9)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (9.0.9)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (9.0.9)
- `CloudinaryDotNet` (1.27.7)
- `Swashbuckle.AspNetCore` (9.0.4)

## 🚀 Deployment

### Publish ứng dụng:
```bash
dotnet publish -c Release -o ./publish
```

### Chạy trên IIS:
1. Publish project
2. Tạo Application Pool trong IIS (.NET CLR Version: No Managed Code)
3. Point đến thư mục publish
4. Cấu hình connection string production trong `appsettings.Production.json`
5. Cài đặt ASP.NET Core Hosting Bundle

### Environment Variables Production:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Production SQL Server Connection String"
  },
  "Jwt": {
    "Key": "Production Secret Key (min 32 chars)",
    "Issuer": "MovieWebApp",
    "Audience": "MovieWebApp"
  },
  "CloudinarySettings": {
    "CloudName": "production-cloud",
    "ApiKey": "production-key",
    "ApiSecret": "production-secret"
  }
}
```

## 🔧 Troubleshooting

### Lỗi CORS
- Đảm bảo `app.UseCors("AllowReactApp")` đặt trước `app.UseAuthorization()`
- Kiểm tra origin trong CORS policy khớp với frontend URL
- Frontend phải gửi `credentials: 'include'` trong fetch requests

### Lỗi 401 Unauthorized
- Kiểm tra JWT token hợp lệ và chưa hết hạn
- Xác nhận header `Authorization: Bearer {token}` được gửi đúng
- Kiểm tra role trong token khớp với `[Authorize(Roles="...")]`

### Lỗi Database Migration
```bash
# Drop database và migrate lại
dotnet ef database drop
dotnet ef database update
```

### Cloudinary Upload Error
- Kiểm tra API credentials trong `appsettings.json`
- Xác nhận file size không vượt quá giới hạn
- Kiểm tra file format (jpg, png, webp)

## 📖 Tài liệu tham khảo

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [JWT Authentication](https://jwt.io/)
- [Cloudinary .NET SDK](https://cloudinary.com/documentation/dotnet_integration)

## 👨‍💻 Tác giả

**Ducanh1911**

## 📄 License

This project is licensed under the MIT License.

---

## 🎯 Features Highlights

### Architecture & Design
✅ **Clean Architecture** (Domain → Application → Infrastructure → Presentation)  
✅ **Repository Pattern** với Generic Repository  
✅ **Dependency Injection** toàn bộ services  
✅ **RESTful API Design** chuẩn HTTP methods  

### Authentication & Security
✅ **JWT Bearer Authentication** với role claims  
✅ **Role-based Authorization** (Admin/User)  
✅ **Password Hashing** với BCrypt  
✅ **CORS Configuration** với credentials support  

### Core Features
✅ **Movie Management**: CRUD với upload poster Cloudinary  
✅ **Search & Pagination**: Tìm kiếm và phân trang movies  
✅ **Rating System**: 1-5 stars với review text  
✅ **Comment System**: Bình luận và thảo luận  
✅ **Favorite System**: Lưu phim yêu thích  
✅ **Genre Management**: Quản lý thể loại phim  

### Admin Features
✅ **Dashboard Statistics**: Tổng quan users, movies, ratings, comments  
✅ **Top Movies**: Top 5 xem nhiều & đánh giá cao  
✅ **User Management**: CRUD users, phân quyền  
✅ **Content Moderation**: Quản lý ratings/comments  

### Technical Features
✅ **Entity Framework Core 9.0** với Code-First Migrations  
✅ **Swagger/OpenAPI Documentation** với JWT support  
✅ **Cloudinary Integration** cho cloud storage  
✅ **Soft Delete Pattern** cho data protection  
✅ **Logging** với ILogger interface  

## 📞 Support

Nếu có vấn đề, vui lòng tạo issue trên GitHub repository.

