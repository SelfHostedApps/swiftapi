using Microsoft.EntityFrameworkCore;

namespace api{

public class Db: DbContext {

        public Db(DbContextOptions<Db> option): base(option) {}

        public DbSet<Data.User> Users { get; set; }
        public DbSet<Data.Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
                builder.Entity<Data.User>().ToTable("users");
                builder.Entity<Data.Tasks>().ToTable("tasks");
        }

}
}
