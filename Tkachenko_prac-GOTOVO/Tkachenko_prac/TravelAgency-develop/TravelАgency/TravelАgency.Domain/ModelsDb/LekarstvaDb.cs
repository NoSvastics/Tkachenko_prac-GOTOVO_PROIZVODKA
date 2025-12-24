using System.ComponentModel.DataAnnotations.Schema;
using TravelАgency.Domain.Enum;
using TravelАgency.Domain.Enum.TravelАgency.Domain.Enum;

namespace TravelАgency.Domain.ModelsDb
{
    [Table("lekarstva")]
    public class LekarstvaDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("manufacturer")]
        public string Manufacturer { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("category")]
        public Category Category { get; set; }

        [Column("path_img")]
        public string PathImg { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }
    }
}