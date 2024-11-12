using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp6
{

    public class AppDbContext : DbContext
    {
        public DbSet<Smartphone> Smartphones { get; set; }

        // Конструктор, который принимает DbContextOptions
        public AppDbContext()
            : base("AppDbContext")
        {

        }

    }
}
    

