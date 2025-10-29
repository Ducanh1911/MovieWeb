# Hướng dẫn sử dụng API Movie Web App

## Các chức năng mới đã được thêm

### 1. Xem thông tin chi tiết phim
- **GET** `/api/Movie/{id}/details` - Xem thông tin chi tiết phim (tự động tăng lượt xem)
- **GET** `/api/Movie/{id}` - Xem thông tin phim cơ bản

### 2. Đánh giá phim (Rating)
- **POST** `/api/Rating` - Tạo đánh giá mới
  ```json
  {
    "movieId": 1,
    "starRating": 5,
    "review": "Phim rất hay!"
  }
  ```
- **PUT** `/api/Rating/{id}` - Cập nhật đánh giá
- **DELETE** `/api/Rating/{id}` - Xóa đánh giá
- **GET** `/api/Rating/movie/{movieId}` - Lấy tất cả đánh giá của phim
- **GET** `/api/Rating/movie/{movieId}/user` - Lấy đánh giá của user hiện tại cho phim
- **GET** `/api/Rating/movie/{movieId}/average` - Lấy điểm trung bình của phim

### 3. Bình luận phim (Comment)
- **POST** `/api/Comment` - Tạo bình luận mới
  ```json
  {
    "movieId": 1,
    "content": "Phim này rất hay, tôi thích!"
  }
  ```
- **PUT** `/api/Comment/{id}` - Cập nhật bình luận
- **DELETE** `/api/Comment/{id}` - Xóa bình luận
- **GET** `/api/Comment/movie/{movieId}` - Lấy tất cả bình luận của phim
- **GET** `/api/Comment/user/{userId}` - Lấy tất cả bình luận của user

### 4. Thông tin phim mở rộng
Phim bây giờ có thêm các trường:
- `trailerUrl` - URL trailer phim
- `director` - Đạo diễn
- `cast` - Diễn viên
- `episodes` - Số tập (cho phim bộ)
- `rating` - Điểm đánh giá trung bình
- `viewCount` - Số lượt xem

## Cách sử dụng

### 1. Xem thông tin phim chi tiết
```bash
GET /api/Movie/1/details
```

### 2. Đánh giá phim
```bash
POST /api/Rating
Authorization: Bearer {token}
Content-Type: application/json

{
  "movieId": 1,
  "starRating": 5,
  "review": "Phim rất hay, tôi thích!"
}
```

### 3. Bình luận phim
```bash
POST /api/Comment
Authorization: Bearer {token}
Content-Type: application/json

{
  "movieId": 1,
  "content": "Phim này rất hay, tôi thích!"
}
```

### 4. Xem đánh giá và bình luận của phim
```bash
GET /api/Rating/movie/1
GET /api/Comment/movie/1
```

## Lưu ý
- Tất cả API đánh giá và bình luận đều yêu cầu authentication
- Mỗi user chỉ có thể đánh giá một phim một lần
- User chỉ có thể sửa/xóa đánh giá và bình luận của chính mình
- Khi tạo/cập nhật/xóa đánh giá, điểm trung bình của phim sẽ được cập nhật tự động

















