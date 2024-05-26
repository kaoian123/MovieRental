using Microsoft.EntityFrameworkCore;

namespace MovieRental.Models.Entities
{
    [PrimaryKey(nameof(GenreID), nameof(Mid))]
    public class MovieGenres
    {
        public int GenreID { get; set; }
        public int Mid { get; set;}
        public required Genres Genres { get; set; }
        public required Movies Movies { get; set; }
    }
}
