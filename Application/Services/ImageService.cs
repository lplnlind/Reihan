using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class ImageService : IImageService
    {

        public async Task<string> UploadImageAsync(IFormFile image, string folderName)
        {
            if (image == null || image.Length == 0)
                throw new AppException("تصویر نامعتبر است", StatusCodes.Status400BadRequest);

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"images/{folderName}/{fileName}";
        }

        public Task DeleteImageAsync(string relativePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
