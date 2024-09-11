using GetCar.DB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;



namespace GetCar.DB.ApplicationDbContext
{
    public class GetCarDbContext:IdentityDbContext<ApplicationUser>
    {
        public GetCarDbContext(DbContextOptions<GetCarDbContext>options):base(options) 
        {
            
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorOwner> VendorOwners { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Invoice>Invoices  { get; set; }
        public DbSet<DropoffLocation> DropoffLocation { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        { 

            base.OnModelCreating(builder);

            builder.Entity<Feedback>()
            .HasOne(f => f.Customer)
            .WithMany(c => c.Feedbacks)
            .HasForeignKey(f => f.CustomerId);

            builder.Entity<Booking>()
            .HasOne(b => b.Invoice)
            .WithOne(i => i.Booking)
            .HasForeignKey<Invoice>(i => i.BookingId);
            // You can add custom role and user configuration here
            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            //    new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" },
            //    new IdentityRole { Name = "Manager", NormalizedName = "MANAGER" },
            //    new IdentityRole { Name = "Finance", NormalizedName = "FINANCE" },
            //    new IdentityRole { Name = "IT", NormalizedName = "IT" },
            //    new IdentityRole { Name = "Vendor", NormalizedName = "VENDOR" },
            //    new IdentityRole { Name = "SubVendor", NormalizedName = "SUBVENDOR" },
            //    new IdentityRole { Name = "Client", NormalizedName = "CLIENT" }
            //);
        }

    }
}
