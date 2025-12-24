using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelАgency.Domain.Filter;
using TravelАgency.Domain.ViewModels.Lekarstva;
using TravelАgency.Service;
using TravelАgency.Service.Interfaces;

namespace TravelАgency.Controllers
{
    public class LekarstvaController : Controller
    {
        private readonly ILekarstvaService _lekarstvaService;
        private IMapper _mapper;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public LekarstvaController(ILekarstvaService lekarstvaService)
        {
            _lekarstvaService = lekarstvaService;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IActionResult ListOfLekarstva()
        {
            var result = _lekarstvaService.GetAllLekarstva();
            var listViewModel = new ListOfLekarstvaViewModel
            {
                LekarstvaList = _mapper.Map<List<LekarstvaViewModel>>(result.Data)
            };

            return View(listViewModel);
        }

        [HttpPost]
        public IActionResult Filter([FromBody] LekarstvaFilter filter)
        {
            var result = _lekarstvaService.GetLekarstvaByFilter(filter);
            var filteredList = _mapper.Map<List<LekarstvaViewModel>>(result.Data);

            return Json(filteredList);
        }

        public async Task<IActionResult> LekarstvaPage(Guid id)
        {
            var resultItem = await _lekarstvaService.GetLekarstvaById(id);
            var resultPics = _lekarstvaService.GetPicturesByIdLekarstva(id);

            var pageModel = _mapper.Map<LekarstvaPageViewModel>(resultItem.Data);
            pageModel.Pictures = _mapper.Map<List<PictureLekarstvaViewModel>>(resultPics.Data);

            return View(pageModel);
        }
    }
}