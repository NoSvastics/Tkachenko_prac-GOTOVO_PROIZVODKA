using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelАgency.Domain.Filter;
using TravelАgency.Domain.ViewModels.Lekarstva;
using TravelАgency.Service.Interfaces;
using TravelАgency.Service;
using TravelАgency.Domain.Enum;
using DomainStatus = TravelАgency.Domain.Enum.StatusCode;

namespace TravelАgency.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly ILekarstvaService _lekarstvaService;
        private readonly IMapper _mapper;

        public MedicinesController(ILekarstvaService lekarstvaService)
        {
            _lekarstvaService = lekarstvaService;
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AppMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpGet]
        public IActionResult Catalog()
        {
            var response = _lekarstvaService.GetAllLekarstva();

            var viewModel = new ListOfLekarstvaViewModel
            {
                LekarstvaList = _mapper.Map<List<LekarstvaViewModel>>(response.Data)
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Filter([FromBody] LekarstvaFilter filter)
        {
            var response = _lekarstvaService.GetLekarstvaByFilter(filter);
            var result = _mapper.Map<List<LekarstvaViewModel>>(response.Data);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _lekarstvaService.GetLekarstvaById(id);
            if (response?.StatusCode == DomainStatus.OK && response.Data != null)
            {
                var viewModel = _mapper.Map<LekarstvaPageViewModel>(response.Data);
                var picsResponse = _lekarstvaService.GetPicturesByIdLekarstva(id);
                if (picsResponse?.Data != null)
                {
                    viewModel.Pictures = _mapper.Map<List<PictureLekarstvaViewModel>>(picsResponse.Data);
                }
                else
                {
                    viewModel.Pictures = new List<PictureLekarstvaViewModel>();
                }

                return View(viewModel);
            }
            return RedirectToAction("Catalog");
        }
    }
}