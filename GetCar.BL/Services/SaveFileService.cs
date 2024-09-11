using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.Services
{
    public class SaveFileService:ISaveFileService
    {
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;


            // Validate file type (e.g., only allowing images)

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
            }

            // Validate file size (e.g., max 10 MB)
            if (file.Length > 10 * 1024 * 1024)
            {
                throw new InvalidOperationException("File size exceeds the maximum limit of 10 MB.");
            }

            // Define the path to the uploads directory relative to wwwroot
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Check if the directory exists, and if not, create it
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            
            var filePath = Path.Combine(uploadsDirectory, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return a relative path for use in URLs
            var relativePath = Path.Combine("uploads", Path.GetFileName(filePath));
            return relativePath;
        }

        public async Task<string> SaveFileDecodeBase64Async(string base64String, string fileName)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            // Define the path to the uploads directory relative to wwwroot
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Check if the directory exists, and if not, create it
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            // Generate a unique filename with the provided file extension
            var filePath = Path.Combine(uploadsDirectory, Guid.NewGuid().ToString() + Path.GetExtension(fileName));

            // Decode the Base64 string into a byte array
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64String);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Invalid Base64 string.");
            }

            // Save the file as binary data to the specified path
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Return a relative path for use in URLs
            var relativePath = Path.Combine("uploads", Path.GetFileName(filePath));
            return relativePath;
        }


    }

}
