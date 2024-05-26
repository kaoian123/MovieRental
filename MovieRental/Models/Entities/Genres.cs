using Microsoft.EntityFrameworkCore;

namespace MovieRental.Models.Entities
{
    [PrimaryKey(nameof(GenreID))]
    public class Genres
    {
        public int GenreID { get; set; }
        public string GenreName { get; set;}
    }
}
