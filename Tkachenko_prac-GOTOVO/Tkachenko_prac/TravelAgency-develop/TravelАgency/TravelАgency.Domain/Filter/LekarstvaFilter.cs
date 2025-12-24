namespace TravelАgency.Domain.Filter
{
    public class LekarstvaFilter
    {
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        public List<string> Categories { get; set; }
    }
}