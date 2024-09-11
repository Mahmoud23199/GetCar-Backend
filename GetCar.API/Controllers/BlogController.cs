using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.BlogsDtos;
using GetCar.BL.GenericRepositry;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IGenericRepositry<Blog> _blogService;
        private readonly ISaveFileService _saveFileService;
        public BlogController(IGenericRepositry<Blog> blogService, ISaveFileService saveFileService)
        {
            _blogService = blogService;
            _saveFileService = saveFileService;
        }

        [HttpGet("GetBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllAsync(null);

            return Ok(new ApiResponse
            {
                Data = blogs,
                StatusCode = StatusCodes.Status200OK,
                Message = "Created Success",
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _blogService.GetByIdAsync(i=>i.Id==id);
            if (blog == null) return NotFound();


            return Ok(new ApiResponse
            {
                Data = blog,
                StatusCode = StatusCodes.Status200OK,
                Message = "Created Success",
            });
        }

        [HttpPost("CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogDto blogDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
            });

            var blog = new Blog
            {
                AuthorName = blogDto.AuthorName,
                AuthorId=blogDto.AuthorId,
                CreatedAt = DateTime.UtcNow,
                Description = blogDto.Description,
                Image=await _saveFileService.SaveFileAsync(blogDto.Image),
                Title = blogDto.Title,
            };
             await _blogService.AddedAsync(blog);

            return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] updateBlogDto blogDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
            });
            var blog = new Blog
            {
                Id=id,
                AuthorId = blogDto.AuthorId,
                AuthorName = blogDto.AuthorName,
                CreatedAt=blogDto.CreatedAt,
                Description = blogDto.Description,
                Image=await _saveFileService.SaveFileAsync(blogDto.Image),
                Title = blogDto.Title,
                
            };
             await _blogService.UpdateAsync(id,blog);
           
                 return Ok(new ApiResponse
                 {
                     Data = blog,
                     StatusCode = StatusCodes.Status200OK,
                     Message = "Created Success",
                 });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {

            var blog =await _blogService.GetByIdAsync(i => i.Id == id, null);
            if(blog == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message=$"Item with id:{id} doesnt exist"
                });
            try
            {
                await _blogService.DeleteByIdAsync(i => i.Id == id);
            }catch(Exception ex)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Item with id:{id} doesnt exist"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }
    }
}
