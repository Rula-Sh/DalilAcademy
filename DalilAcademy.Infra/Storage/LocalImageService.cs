using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Storage;
using Microsoft.Extensions.Options;

namespace DalilAcademy.Infra.Storage
{
    // Infra/Storage/LocalImageService.cs
    public class LocalImageService(IOptions<ImageStorageOptions> options) : IImageService
    {
        private readonly string _folder = Path.Combine(options.Value.RootPath, "images", "courses");
        private const long MaxBytes = 2 * 1024 * 1024; // 2 MB
        private static readonly Dictionary<string, byte[]> Signatures = new()
        {
            [".jpg"] = [0xFF, 0xD8, 0xFF],
            [".jpeg"] = [0xFF, 0xD8, 0xFF],
            [".png"] = [0x89, 0x50, 0x4E, 0x47],
            [".webp"] = [0x52, 0x49, 0x46, 0x46]  // "RIFF"
        };

        public async Task<string> SaveCourseImageAsync(Stream content, string fileName, long length)
        {
            if (length is 0 or > MaxBytes)
                throw new InvalidFileException("Image must be between 1 byte and 2 MB.");

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!Signatures.TryGetValue(extension, out var signature))
                throw new InvalidFileException("Only jpg, jpeg, png, and webp images are allowed.");

            var header = new byte[signature.Length];
            await content.ReadExactlyAsync(header);
            if (!header.SequenceEqual(signature)) // the extension lied
                throw new InvalidFileException("The file content does not match its extension.");
            content.Position = 0;

            var name = $"{Guid.NewGuid()}{extension}"; // Guid.NewGuid() generates a new id, its best not to include the actual file name

            Directory.CreateDirectory(_folder); // create folder if it does not exist

            await using var file = File.Create(Path.Combine(_folder, name));
            await content.CopyToAsync(file);

            return $"/images/courses/{name}";
        }
        public void DeleteCourseImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var path = Path.Combine(_folder, Path.GetFileName(imageUrl));
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // simpler approach:
    /* public class LocalImageService : IImageService
    {
        private readonly string _folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");
        private static readonly string[] Allowed = [".jpg", ".jpeg", ".png", ".webp"];

        public async Task<string> SaveCourseImageAsync(Stream content, string fileName, long length)
        {
            if (length == 0 || length > 2 * 1024 * 1024)
                throw new InvalidFileException("Image must be less then 2 MB");

            var extension = Path.GetExtension(fileName).ToLower();
            if (!Allowed.Contains(extension))
                throw new InvalidFileException("Image extension must be one of the following: .jpg .jpeg .png .webp");

            Directory.CreateDirectory(_folder); // creates the folder if it does not exist
            
            var name = $"{{Guid.NewGuid()}{extension}";
            await using var file = File.Create(Path.Combine(_folder, name)); // creates an empty file
            await content.CopyToAsync(file); // copies the content of the image to the file

            return "/images/courses/" + name;
        }
        public void DeleteCourseImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var path = Path.Combine(_folder, Path.GetFileName(imageUrl));
            if (File.Exists(path))
                File.Delete(path);
        }
    } */
}
