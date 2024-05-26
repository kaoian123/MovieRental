using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieRental.Models.Entities
{
    [PrimaryKey(nameof(Mid))]
    public class Movies
    {
        public int Mid { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Summary { get; set; }
        public float Rating { get; set; }
        public string BoxOfficeMoney { get; set; }
        public int MovieStatus { get; set; }
    }
}
