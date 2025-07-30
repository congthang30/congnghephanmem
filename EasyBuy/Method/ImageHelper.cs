using Microsoft.AspNetCore.Http;

public static class ImageHelper
{
    public static async Task<string?> SaveImageAsync(IFormFile image, string subFolder = "ratings", int maxSizeMb = 5)
    {
        if (image == null || image.Length == 0)
            return null;

        var ext = Path.GetExtension(image.FileName).ToLower();
        var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        if (!allowedExt.Contains(ext))
            throw new Exception("Chỉ hỗ trợ ảnh định dạng JPG, JPEG, PNG, GIF.");

        if (image.Length > maxSizeMb * 1024 * 1024)
            throw new Exception($"Ảnh không được vượt quá {maxSizeMb}MB.");

        var fileName = $"{Guid.NewGuid()}{ext}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", subFolder);
        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/images/{subFolder}/{fileName}";
    }
}
