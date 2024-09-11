using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.CustomResponse
{
    public class ApiResponse
    {
        public object Data { get; set; }
        public int StatusCode { get; set; }
        public object ?Errors { get; set; } 
        public string ?Message { get; set; }
    }
}
