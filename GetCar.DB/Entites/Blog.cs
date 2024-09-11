using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }

    
}
