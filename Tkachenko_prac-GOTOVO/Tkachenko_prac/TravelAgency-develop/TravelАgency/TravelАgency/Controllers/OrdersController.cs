using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelАgency.Service.Interfaces;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.ModelsDb;
using Microsoft.EntityFrameworkCore;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.Enum;
using DomainStatus = TravelАgency.Domain.Enum.StatusCode;

namespace TravelАgency.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IZakazService _zakazService;
        private readonly ILekarstvaService _lekarstvaService;
        private readonly IBaseStorage<UserDb> _userStorage;

        public OrdersController(
            IZakazService zakazService,
            ILekarstvaService lekarstvaService,
            IBaseStorage<UserDb> userStorage)
        {
            _zakazService = zakazService;
            _lekarstvaService = lekarstvaService;
            _userStorage = userStorage;
        }

        private async Task<Guid?> GetCurrentUserIdAsync()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idClaim) && Guid.TryParse(idClaim, out var guid))
                return guid;

            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var userDb = await _userStorage.GetAll().FirstOrDefaultAsync(u => u.Email == email);
                if (userDb != null)
                    return userDb.Id;
            }

            return null;
        }

        public async Task<IActionResult> List()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
                return RedirectToAction("SiteInformation", "Home");

            var userId = await GetCurrentUserIdAsync();
            if (userId == null)
                return RedirectToAction("SiteInformation", "Home");

            var result = await _zakazService.GetUserOrders(userId.Value);
            
            var ordersWithMedicines = new List<dynamic>();
            if (result.Data != null)
            {
                foreach (var order in result.Data)
                {
                    var medicineResult = await _lekarstvaService.GetLekarstvaById(order.IdLekarstva);
                    ordersWithMedicines.Add(new
                    {
                        Order = order,
                        Medicine = medicineResult?.Data
                    });
                }
            }

            return View(ordersWithMedicines);
        }

        // DTO для удаления (привязка из JSON тела)
        public class DeleteOrderDto
        {
            public Guid Id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderDto dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                return BadRequest("Неверный идентификатор заказа.");

            var userId = await GetCurrentUserIdAsync();
            if (userId == null)
                return Unauthorized();

            // Проверяем существование заказа и владельца
            var orderResp = await _zakazService.GetById(dto.Id);
            if (orderResp == null || orderResp.Data == null || orderResp.StatusCode != DomainStatus.OK)
                return NotFound("Заказ не найден.");

            if (orderResp.Data.IdUser != userId.Value)
                return Forbid();

            var result = await _zakazService.Delete(dto.Id);
            
            if (result.StatusCode == DomainStatus.OK)
                return Ok(new { success = true });

            return BadRequest(new { success = false, message = result.Description });
        }

        // DTO для создания заказа (остался)
        public class CreateOrderDto
        {
            public Guid Id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == null)
                return Unauthorized();

            if (dto == null || dto.Id == Guid.Empty)
                return BadRequest("Неверный идентификатор препарата.");

            var medResp = await _lekarstvaService.GetLekarstvaById(dto.Id);
            if (medResp == null || medResp.Data == null || medResp.StatusCode != DomainStatus.OK)
                return BadRequest("Лекарство не найдено.");

            var newOrder = new Zakaz
            {
                Id = Guid.NewGuid(),
                IdUser = userId.Value,
                IdLekarstva = dto.Id,
                Price = medResp.Data.Price,
                CreatedAt = DateTime.Now
            };

            var createResp = await _zakazService.Create(newOrder);
            if (createResp.StatusCode == DomainStatus.OK)
                return Ok(new { success = true });

            return BadRequest(createResp.Description ?? "Не удалось добавить в корзину.");
        }
    }
}