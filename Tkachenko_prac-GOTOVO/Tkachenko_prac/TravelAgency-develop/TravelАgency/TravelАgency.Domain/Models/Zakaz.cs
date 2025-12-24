namespace TravelАgency.Domain.Models
{
    public class Zakaz
    {
        public Guid Id { get; set; }

        public Guid IdUser { get; set; }

        public Guid IdLekarstva { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}