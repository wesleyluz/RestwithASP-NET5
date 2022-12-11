using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext() 
        {

        }
        public MySqlContext(DbContextOptions<MySqlContext> options) : base (options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Book> Library { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
