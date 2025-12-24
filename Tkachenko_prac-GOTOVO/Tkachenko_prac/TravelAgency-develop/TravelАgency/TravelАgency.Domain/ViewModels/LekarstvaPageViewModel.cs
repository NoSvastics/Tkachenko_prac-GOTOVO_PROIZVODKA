using TravelАgency.Domain.Enum;
using TravelАgency.Domain.Enum.TravelАgency.Domain.Enum;

namespace TravelАgency.Domain.ViewModels.Lekarstva
{
    public class LekarstvaPageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public string PathImg { get; set; }
        public List<PictureLekarstvaViewModel> Pictures { get; set; }
    }

    public class PictureLekarstvaViewModel
    {
        public string PathImg { get; set; }
    }
}