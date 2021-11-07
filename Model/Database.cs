using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Database: DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorRate> AuthorsRates { get; set; }
        public DbSet<BookRate> BooksRates { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=D:\\studia\\ProgramowanieUzytkoweIP12\\Model\\database.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasDiscriminator(x => x.Type)
                .HasValue<AuthorRate>(RateType.AuthorRate)
                .HasValue<BookRate>(RateType.BookRate);
        }
    }
}
