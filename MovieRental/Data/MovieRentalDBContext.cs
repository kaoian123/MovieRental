using Microsoft.EntityFrameworkCore;
using MovieRental.Models.Entities;

namespace MovieRental.Data
{
    public class MovieRentalDBContext(DbContextOptions<MovieRentalDBContext> options) : DbContext(options)
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<MovieGenres> MovieGenres { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<RentalRecords> RentalRecords { get; set; }
    }
}
