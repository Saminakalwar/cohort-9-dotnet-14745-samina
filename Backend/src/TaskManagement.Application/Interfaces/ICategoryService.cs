
using TaskManagement.Application.DTOs.Categories;
namespace TaskManagement.Application.Interfaces;
public interface ICategoryService
{
    Task<Guid> CreateCategoryAsync(CreateCategoryRequest request);
    Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
    }