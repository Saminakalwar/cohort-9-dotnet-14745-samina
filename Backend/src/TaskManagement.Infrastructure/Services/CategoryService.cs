using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.Interfaces;
using TaskManagement.Persistence.Context;
using TaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
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
        _logger.LogInformation("Category {CategoryId} created successfully : {CategoryName}", category.Id, category.Name);
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