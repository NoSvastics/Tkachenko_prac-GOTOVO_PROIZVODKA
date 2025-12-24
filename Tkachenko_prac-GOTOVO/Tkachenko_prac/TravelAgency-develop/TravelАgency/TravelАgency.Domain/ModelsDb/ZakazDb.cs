using System.ComponentModel.DataAnnotations.Schema;

namespace TravelАgency.Domain.ModelsDb
{
    [Table("zakaz")]
    public class ZakazDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_user")]
        public Guid IdUser { get; set; }

        [Column("id_lekarstva")]
        public Guid IdLekarstva { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }
    }
}