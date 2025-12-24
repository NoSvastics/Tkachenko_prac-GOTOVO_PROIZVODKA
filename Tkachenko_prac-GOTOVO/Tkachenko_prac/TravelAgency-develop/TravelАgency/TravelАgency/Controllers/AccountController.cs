using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelAgency.Service.Interfaces;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.ViewModels.LoginAndRegistration;
using AutoMapper;

namespace TravelАgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<TravelАgency.Service.AppMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var response = await _accountService.Login(user);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(response.Data));

                    return RedirectToAction("SiteInformation", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("SiteInformation", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(string.Join(";", errors.Select(e => e.ErrorMessage)));
            }

            var user = new User
            {
                Login = model.Login,
                Email = model.Email,
                Password = model.Password
            };

            try
            {
                var response = await _accountService.Register(user);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { code = response.Data, message = response.Description });
                }
                return BadRequest(response.Description);
            }
            catch (Exception ex)
            {
                // на время разработки вернём текст ошибки — позже логируй через ILogger
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var response = await _accountService.ConfirmEmail(user, model.GeneratedCode, model.CodeConfirm);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));
                return Ok();
            }
            return BadRequest(response.Description);
        }

        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction("SiteInformation", "Home");

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("SiteInformation", "Home");

            // Проверить или создать пользователя в БД
            var user = new User
            {
                Login = name ?? email.Split('@')[0],
                Email = email,
                Password = null // Google пользователи без пароля
            };

            // Изменено: для Google-авторизации вызываем IsCreatedAccount, чтобы создать пользователя при необходимости
            var response = await _accountService.IsCreatedAccount(user);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));
            }

            return RedirectToAction("SiteInformation", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SiteInformation", "Home");
        }
    }
}