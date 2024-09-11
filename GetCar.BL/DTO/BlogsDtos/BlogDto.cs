using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.BlogsDtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class updateBlogDto
    {
        public string AuthorName { get; set; }
        public string? AuthorId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
