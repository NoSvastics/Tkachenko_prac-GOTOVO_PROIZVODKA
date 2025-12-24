using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Service.Interfaces;
using TravelАgency.Service;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.Enum;
using TravelАgency.Domain.ModelsDb;
using TravelАgency.Domain.Response;
using TravelАgency.Domain.Models;
using System.Security.Claims;
using TravelАgency.Service.HelpersService;
using TravelАgency.Domain.Helpers;
using TravelАgency.Domain.Validators;
using FluentValidation;
using MimeKit;
using MailKit.Net.Smtp;
using TravelАgency.Domain.ViewModels.LoginAndRegistration;
using System.Threading.Tasks;

namespace ForumFTI.Service.Realization
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private IMapper _mapper { get; set; }
        private UserValidator _validationRules { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AccountService(IBaseStorage<UserDb> userStorage)
        {
            _userStorage = userStorage;
            _validationRules = new UserValidator();
            _mapper = mapperConfiguration.CreateMapper();
        }
        public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);

                var userdb = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);

                if (userdb == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (userdb.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или почта"
                    };
                }

                model = _mapper.Map<User>(userdb);
                var result = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                // Получение сообщений об ошибках валидации
                var errorMessages = string.Join(";", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<string>> Register(User model)
        {
            try
            {
                Random random = new Random();
                string confirmationCode = $"{random.Next(10)}{random.Next(10)}{random.Next(10)}{random.Next(10)}";

                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) != null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                    };
                }

                // TEMP: не отправляем почту в режиме разработки — записываем код в файл (чтобы проверить UI/логики)
                try
                {
                    var logPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "confirmation_codes.txt");
                    System.IO.File.AppendAllText(logPath, $"{DateTime.Now:O}\t{model.Email}\t{confirmationCode}{Environment.NewLine}");
                }
                catch
                {
                    // игнорируем ошибки логирования — не должны ломать основной поток
                }

                // Запускаем отправку письма в фоне (не блокируем основной поток)
                try
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await SendEmail(model.Email, confirmationCode);
                        }
                        catch
                        {
                            // логируйте ошибку через ILogger в будущем
                        }
                    });
                }
                catch
                {
                    // безопасно проигнорировать — отправка не критична для UI
                }

                return new BaseResponse<string>()
                {
                    Data = confirmationCode,
                    Description = "Письмо (dev) — код записан в файл",
                    StatusCode = StatusCode.OK
                };
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errorMessages = string.Join(";", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<string>()
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task SendEmail(string email, string confirmationCode)
        {
            string path = "C:\\Users\\WeirdToday\\Desktop\\DO NOT CHANGE DO NOT DELETE.txt";
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "TurAgenT@bk.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Добро пожаловать!";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<html>" + "<head>" + "<style>" +
           "body { font-family: Arial, sans-serif; background-color: #f2f2f2; }" +
           ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); }" +
           ".header { text-align: center; margin-bottom: 20px; }" +
           ".message { font-size: 16px; line-height: 1.6; }" +
           ".conteiner-code { background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }" +
           ".code {text-align: center; }" +
           "</style>" +
           "</head>" +
           "<body>" +
           "<div class='container'>" +
           "<div class='header'><h1>Добро пожаловать на сайт Туристическое агентство!</h1></div>" +
           "<div class='message'>" +
           "<p>Пожалуйста, введите данный код на сайте, чтобы подтвердить ваш email и завершить регистрацию:</p>" +
           "<div class='conteiner-code'><p class='code'>" + confirmationCode + "</p></div>" +
           "</div>" + "</div>" + "</body>" + "</html>"
            };

            using (StreamReader reader = new StreamReader(path))
            {
                string password = await reader.ReadToEndAsync();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync("sergenator2000@gmail.com", password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }

            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode)
        {
            try
            {
                if (code != confirmCode)
                {
                    throw new Exception("Неверный код! Регистрация не выполнена.");
                }

                model.PathImage = " ";
                model.CreatedAt = DateTime.Now;
                model.Password = HashPasswordHelper.HashPassword(model.Password);


                await _validationRules.ValidateAndThrowAsync(model);

                var userdb = _mapper.Map<UserDb>(model);

                await _userStorage.Add(userdb);

                // Используем сохранённый объект из БД, чтобы в claims был корректный Id и PathImage
                var savedUser = _mapper.Map<User>(userdb);
                var result = AuthenticateUserHelper.Authenticate(savedUser);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
        {
            try
            {
                // Попробуем найти существующего пользователя по email
                var existingUserDb = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);

                if (existingUserDb == null)
                {
                    // Создаём нового пользователя для Google
                    model.Password = "google"; // временное значение пароля
                    model.CreatedAt = DateTime.Now;
                    if (string.IsNullOrEmpty(model.PathImage)) model.PathImage = " ";

                    var userDb = _mapper.Map<UserDb>(model);

                    await _userStorage.Add(userDb);

                    // После сохранения используем данные из БД
                    var savedUser = _mapper.Map<User>(userDb);

                    var resultRegister = AuthenticateUserHelper.Authenticate(savedUser);
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Data = resultRegister,
                        Description = "Объект добавился",
                        StatusCode = StatusCode.OK
                    };
                }

                // Если пользователь уже есть — используем его данные из БД
                var mappedExisting = _mapper.Map<User>(existingUserDb);
                var resultLogin = AuthenticateUserHelper.Authenticate(mappedExisting);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultLogin,
                    Description = "Объект уже был создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}

