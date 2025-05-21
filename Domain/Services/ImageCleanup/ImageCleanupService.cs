using DAL;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Domain
{
    public class ImageCleanupService : IJob
    {
        private readonly ClothesMarketplaceDbContext _context;
        private readonly string _imagesPath;

        public ImageCleanupService(ClothesMarketplaceDbContext context)
        {
            _context = context;
            _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await CleanupImages();
        }

        public async Task CleanupImages()
        {
            try
            {
                var dbImageNames = new HashSet<string>();

                // Collect image names from Images table (related to Products)
                var productImages = await _context.ProductImages
                    .Where(i => !string.IsNullOrEmpty(i.ImageName))
                    .Select(i => i.ImageName) // No Path.GetFileName since DB stores name without extension
                    .ToListAsync();
                dbImageNames.UnionWith(productImages);

                // Collect image names from Category
                var categoryImages = await _context.Categories
                    .Where(c => !string.IsNullOrEmpty(c.ImageName))
                    .Select(c => c.ImageName) // No Path.GetFileName since DB stores name without extension
                    .ToListAsync();
                dbImageNames.UnionWith(categoryImages);

                // Collect image names from Brand
                var brandImages = await _context.Brands
                    .Where(b => !string.IsNullOrEmpty(b.ImageName))
                    .Select(b => b.ImageName) // No Path.GetFileName since DB stores name without extension
                    .ToListAsync();
                dbImageNames.UnionWith(brandImages);

                // Check if images directory exists
                if (!Directory.Exists(_imagesPath))
                {
                    Console.WriteLine("Images directory does not exist.");
                    return;
                }

                // Get list of files on server (without extensions for comparison)
                var serverImageNames = Directory
                    .GetFiles(_imagesPath)
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();

                // Find files to delete (compare without extensions)
                var filesToDelete = Directory
                    .GetFiles(_imagesPath)
                    .Where(file => !dbImageNames.Contains(Path.GetFileNameWithoutExtension(file)))
                    .ToList();

                // Delete files
                foreach (var file in filesToDelete)
                {
                    var filePath = Path.Combine(_imagesPath, Path.GetFileName(file));
                    try
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"File removed: {Path.GetFileName(file)}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {Path.GetFileName(file)}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while cleaning images: {ex.Message}");
            }
        }
    }
}