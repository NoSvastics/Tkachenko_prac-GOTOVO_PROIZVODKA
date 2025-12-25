using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Travel¿gency.Service.Interfaces;
using Travel¿gency.Domain.Models;
using AutoMapper;
using Travel¿gency.Service;

namespace Travel¿gency.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILekarstvaService _lekarstvaService;
        private readonly IMapper _mapper;

        public AdminController(ILekarstvaService lekarstvaService)
        {
            _lekarstvaService = lekarstvaService;
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AppMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        // ===== ”œ–¿¬À≈Õ»≈ À≈ ¿–—“¬¿Ã» =====
        public IActionResult Medicines()
        {
            var response = _lekarstvaService.GetAllLekarstva();
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult CreateMedicine()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicine(Lekarstva model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;
                
                var response = await _lekarstvaService.AddLekarstva(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Medicines");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditMedicine(Guid id)
        {
            var response = await _lekarstvaService.GetLekarstvaById(id);
            if (response?.StatusCode == Domain.Enum.StatusCode.OK && response.Data != null)
            {
                return View(response.Data);
            }
            return RedirectToAction("Medicines");
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(Lekarstva model)
        {
            if (ModelState.IsValid)
            {
                var response = await _lekarstvaService.UpdateLekarstva(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Medicines");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

[HttpPost]
        public async Task<IActionResult> DeleteMedicine(Guid id)
        {
            var response = await _lekarstvaService.DeleteLekarstva(id);
            return RedirectToAction("Medicines");
        }

        // ===== ”œ–¿¬À≈Õ»≈ «¿ ¿«¿Ã» =====
        public IActionResult Orders()
        {
            return View();
        }

        // ===== ”œ–¿¬À≈Õ»≈ œŒÀ‹«Œ¬¿“≈ÀﬂÃ» =====
        public IActionResult Users()
        {
            return View();
        }

        // ===== —“¿“»—“» ¿ =====
        public IActionResult Statistics()
        {
            return View();
        }
    }
}
