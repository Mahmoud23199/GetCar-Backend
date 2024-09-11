using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Admin:ApplicationUser
    {
        public ICollection<Employee> Employees { get; set; }
    }
}
