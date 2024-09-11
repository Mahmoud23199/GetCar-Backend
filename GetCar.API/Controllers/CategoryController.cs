using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO;
using GetCar.BL.DTO.CarDTOs;
using GetCar.BL.DTO.CategoryDtos;
using GetCar.BL.GenericRepositry;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepositry<Category> _categoryRepository;
        private readonly ICategoryRepositry _customCategoryRepository;

        public CategoryController(IGenericRepositry<Category> categoryRepository, ICategoryRepositry customCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _customCategoryRepository= customCategoryRepository;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(InsertCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                var response = new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                };

                return BadRequest(response);
            }
            var categore = new Category
            {
                CategoryName = model.Name,
                Description = model.Description
            };
            await _categoryRepository.AddedAsync(categore);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categore.CategoryID }, categore);
        }

        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            string[] include = { "Cars" };
            var category = await _categoryRepository.GetByIdAsync(c => c.CategoryID == id,include);

            if (category == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = "Category Not Found"
                });

            var data = new getCategoryDto
            {
                Id = category.CategoryID,
                Name = category.CategoryName,
                Description = category.Description,
                
                
            };

            return Ok(new ApiResponse
            {
                Data =new { data,
                items= new 
                {
                    cars=category.Cars.Select(i=>new {
                     name=i.Name,
                     category=i.Category.CategoryName,
                     pricePerDay=i.PricePerDay,
                     availability=i.Availability,
                     liter=i.Liter,
                     doors=i.Doors,
                     type=i.Type,
                     people=i.People,
                     model=i.Model,

                    }),
                } 
                },
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        [HttpGet("GetCategorys")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            string[] include = { "Cars" };

            var data = await _categoryRepository.GetAllAsync(include);

            var response = new
            {
                Data = data.Select(i => new getCategoryDto
                {
                    Id = i.CategoryID,
                    Name = i.CategoryName,
                    Description = i.Description
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize),
                StatusCode = StatusCodes.Status200OK,
                TotalPages = Math.Ceiling((decimal)data.Count() / pageSize),
                ItemsCountInEachPage = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).Count()
            };
            return Ok(response);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InsertCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(c => c.CategoryID == id);
                if (existingCategory == null)
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Category not found"
                    });
                }

                existingCategory.CategoryName = model.Name;
                existingCategory.Description = model.Description;

                await _customCategoryRepository.UpdateAsync(existingCategory);

                return Ok(new ApiResponse
                {
                    Data = existingCategory,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Updated Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("deleteCategory/{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(c => c.CategoryID == id);
            if (existingCategory == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Category not found"
                });
            }
            await _customCategoryRepository.DeleteCategoryAsync(existingCategory);//delete Caetgory and make cars that related to it with categorId null

            return Ok(new ApiResponse
            {
                Data = existingCategory,
                StatusCode = StatusCodes.Status200OK,
                Message = "Deleted Successfully"
            });

        }

    }
}
