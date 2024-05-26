using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental.Models.Entities
{
    [PrimaryKey(nameof(RecordID))]
    public class RentalRecords
    {
        public int RecordID { get; set; }
        public string RentalDate { get; set; }
        public string ReturnDate { get; set; }
        public string ActualReturnDate { get; set; }
        public int RentalStatus { get; set; }
        [ForeignKey(nameof(Member.Uid))]
        public  Member Member { get; set; }
        [ForeignKey(nameof(Movies.Mid))]
        public  Movies Movies { get; set; }
    }
}
