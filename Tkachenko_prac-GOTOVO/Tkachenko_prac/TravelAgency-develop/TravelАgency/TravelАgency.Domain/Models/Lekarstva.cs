using TravelАgency.Domain.Enum;
using TravelАgency.Domain.Enum.TravelАgency.Domain.Enum;

namespace TravelАgency.Domain.Models
{
    public class Lekarstva
    {
        public Guid Id { get; set; }

        public string Name { get; set; } // Название лекарства

        public string Manufacturer { get; set; } // Производитель

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public string PathImg { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}