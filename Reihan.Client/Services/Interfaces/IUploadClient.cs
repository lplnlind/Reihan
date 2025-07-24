using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IUploadClient
    {
        Task<ImageUploadResult> UploadProductImageAsync(MultipartFormDataContent content);
        Task<ImageUploadResult> UploadProfileImageAsync(MultipartFormDataContent content);
        Task DeleteImageAsync(string path);
        Task DeleteImageByUserAsync(string path);
    }
}
