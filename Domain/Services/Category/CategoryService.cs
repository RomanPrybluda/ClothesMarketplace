using DAL;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class CategoryService
    {
        private readonly ClothesMarketplaceDbContext _context;
        private readonly IImageService _imageService;

        public CategoryService(ClothesMarketplaceDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (!categories.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No categories found");

            var categoryDTOs = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var categoryDTO = CategoryDTO.FromCategory(category);
                categoryDTOs.Add(categoryDTO);
            }

            return categoryDTOs;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id)
        {
            var categoryById = await _context.Categories.FindAsync(id);

            if (categoryById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            var categoryDTO = CategoryDTO.FromCategory(categoryById);

            return categoryDTO;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO request)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == request.Name);

            if (existingCategory != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Category '{request.Name}' already exists.");

            var category = CreateCategoryDTO.ToCategory(request);
            var imageName = await _imageService.UploadImageAsync(request.Image);
            category.ImageName = imageName;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var createdCategory = await _context.Categories.FindAsync(category.Id);
            var categoryDTO = CategoryDTO.FromCategory(createdCategory);

            return categoryDTO;
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid id, UpdateCategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");
            category.ImageName = await _imageService.UploadImageAsync(request.Image);

            request.UpdateCategory(category);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            var categoryDTO = CategoryDTO.FromCategory(category);

            return categoryDTO;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
