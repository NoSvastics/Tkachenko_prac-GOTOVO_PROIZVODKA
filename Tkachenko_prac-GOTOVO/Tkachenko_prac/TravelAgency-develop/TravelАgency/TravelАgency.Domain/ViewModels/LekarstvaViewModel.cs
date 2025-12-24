using TravelАgency.Domain.Enum;
using TravelАgency.Domain.Enum.TravelАgency.Domain.Enum;

namespace TravelАgency.Domain.ViewModels.Lekarstva
{
    public class ListOfLekarstvaViewModel
    {
        public List<LekarstvaViewModel> LekarstvaList { get; set; }
    }

    public class LekarstvaViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public string PathImg { get; set; }
    }
}