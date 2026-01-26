using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest Request)
        {
            var category = Request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesForAdmin()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;
        }

        public async Task<List<CategoryUserResponse>> GetAllCategoriesForUser(string lang = "en")
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories.BuildAdapter()
                .AddParameters("lang", lang).AdaptToType<List<CategoryUserResponse>>();
            return response;
        }


        public async Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest Request)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found",
                    };
                }
                if (Request.Translations != null)
                {
                    foreach (var translation in Request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == translation.Language);

                        if (existing is not null)
                        {
                            existing.Name = translation.Name;
                        }
                        else
                        {
                            return new BaseResponse
                            {
                                Success = false,
                                Message = $"Translation for language {translation.Language} not found",
                            };
                        }


                    }
                }


                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category updated successfully",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cannot Update Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> ToggleStatus(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found",
                    };
                }
                category.Status = category.Status == Status.Active ? Status.Inactive : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category status toggled successfully",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cannot Toggle Category Status",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found",
                    };
                }
                await _categoryRepository.DeleteAsync(category);

                return new BaseResponse
                {
                    Success = true,
                    Message = "Category deleted successfully",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cannot Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}

