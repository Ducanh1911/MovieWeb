using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace MovieWebApp.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "movies/posters")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File ảnh không hợp lệ");

            // Validate image file
            var allowedImageTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!allowedImageTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Chỉ chấp nhận file ảnh (jpg, png, gif, webp)");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = new Transformation().Width(1920).Height(1080).Crop("limit").Quality("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            if (uploadResult.Error != null)
                throw new Exception($"Lỗi upload ảnh: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string> UploadVideoAsync(IFormFile file, string folder = "movies/videos")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File video không hợp lệ");

            // Validate video file
            var allowedVideoTypes = new[] { "video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/webm" };
            if (!allowedVideoTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Chỉ chấp nhận file video (mp4, mpeg, mov, avi, webm)");

            // Giới hạn size video (500MB)
            const long maxVideoSize = 500 * 1024 * 1024;
            if (file.Length > maxVideoSize)
                throw new ArgumentException("File video không được vượt quá 500MB");

            await using var stream = file.OpenReadStream();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            if (uploadResult.Error != null)
                throw new Exception($"Lỗi upload video: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }
    }

    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
