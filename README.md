# 🎬 MovieWebApp - Backend API

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
- `POST /logout` - Đăng xuất

### 🎬 Movie - Client (`/api/Client/Movie`)
- `GET /` - Lấy danh sách phim
- `GET /{id}` - Lấy thông tin phim cơ bản
- `GET /{id}/details` - Lấy chi tiết phim (tăng view count)
- `GET /search?keyword={keyword}` - Tìm kiếm phim
- `GET /genre/{genreId}` - Lấy phim theo thể loại

### 🎭 Genre - Client (`/api/Client/Genre`)
- `GET /` - Lấy danh sách thể loại

### ⭐ Rating (`/api/Rating`)
- `POST /` - Tạo đánh giá
- `PUT /{id}` - Cập nhật đánh giá
- `DELETE /{id}` - Xóa đánh giá
- `GET /movie/{movieId}` - Lấy đánh giá của phim
- `GET /movie/{movieId}/user` - Lấy đánh giá của user
- `GET /movie/{movieId}/average` - Lấy điểm trung bình

### 💬 Comment (`/api/Comment`)
- `POST /` - Tạo bình luận
- `PUT /{id}` - Cập nhật bình luận
- `DELETE /{id}` - Xóa bình luận
- `GET /movie/{movieId}` - Lấy bình luận của phim
- `GET /user/{userId}` - Lấy bình luận của user

### ❤️ Favorite (`/api/Favorite`)
- `POST /` - Thêm phim yêu thích
- `DELETE /{id}` - Xóa phim yêu thích
- `GET /user` - Lấy danh sách phim yêu thích của user

### 👑 Admin - Movie (`/api/Admin/Movie`)
- `GET /` - Lấy tất cả phim (bao gồm đã xóa)
- `POST /` - Tạo phim mới
- `PUT /{id}` - Cập nhật phim
- `DELETE /{id}` - Xóa phim (soft delete)

### 👑 Admin - User (`/api/Admin/User`)
- `GET /users` - Lấy danh sách người dùng
- `GET /users/{id}` - Lấy thông tin user
- `PUT /users/{id}/role` - Cập nhật role
- `DELETE /users/{id}` - Xóa user

### 👑 Admin - Dashboard (`/api/Admin`)
- `GET /dashboard` - Thống kê tổng quan
- `GET /ratings` - Lấy tất cả đánh giá
- `DELETE /ratings/{id}` - Xóa đánh giá
- `GET /comments` - Lấy tất cả bình luận
- `DELETE /comments/{id}` - Xóa bình luận

### 👑 Admin - Genre (`/api/Admin/Genre`)
- `POST /` - Tạo thể loại
- `PUT /{id}` - Cập nhật thể loại
- `DELETE /{id}` - Xóa thể loại

## 🔧 Các Service chính

- **MovieService**: Quản lý phim, tìm kiếm, thống kê lượt xem
- **AuthService**: Xác thực, đăng ký, đăng nhập, JWT token
- **RatingService**: Quản lý đánh giá, tính điểm trung bình
- **CommentService**: Quản lý bình luận
- **FavoriteService**: Quản lý phim yêu thích
- **GenreService**: Quản lý thể loại
- **CloudinaryService**: Upload/Delete poster lên Cloudinary

## 🛡️ Security Features

- **JWT Authentication**: Bảo mật API endpoints
- **Role-based Authorization**: Phân quyền Admin/User
- **Password Hashing**: Mã hóa mật khẩu
- **CORS Policy**: Chỉ cho phép truy cập từ frontend (localhost:3000)
- **Soft Delete**: Xóa mềm dữ liệu quan trọng

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
2. Tạo Application Pool trong IIS
3. Point đến thư mục publish
4. Cấu hình connection string production

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

✅ Clean Architecture  
✅ JWT Authentication  
✅ Role-based Authorization  
✅ RESTful API Design  
✅ Entity Framework Core  
✅ Swagger Documentation  
✅ Cloudinary Integration  
✅ CRUD Operations  
✅ Search & Filter  
✅ Rating & Comment System  
✅ Favorite Movies  
✅ Admin Dashboard  
✅ Soft Delete  

## 📞 Support

Nếu có vấn đề, vui lòng tạo issue trên GitHub repository.
