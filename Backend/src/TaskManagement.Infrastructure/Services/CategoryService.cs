using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.Interfaces;
using TaskManagement.Persistence.Context;
using TaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
    {
      return await _context.Categories.Select(category => new CategoryResponse
      {
          Id = category.Id,
          Name = category.Name
      }).ToListAsync();
    }
}