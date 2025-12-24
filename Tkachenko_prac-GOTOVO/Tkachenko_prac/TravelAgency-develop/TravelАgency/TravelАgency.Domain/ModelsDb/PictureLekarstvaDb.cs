using System.ComponentModel.DataAnnotations.Schema;

namespace TravelАgency.Domain.ModelsDb
{
    [Table("pictures_lekarstva")]
    public class PictureLekarstvaDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_lekarstva")]
        public Guid IdLekarstva { get; set; }

        [Column("path_img")]
        public string PathImg { get; set; }
    }
}