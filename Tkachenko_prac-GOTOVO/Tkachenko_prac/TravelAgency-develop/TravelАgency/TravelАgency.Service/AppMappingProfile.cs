using AutoMapper;
using TravelАgency.Domain.ModelsDb;
using TravelАgency.Domain.ViewModels.LoginAndRegistration;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.ViewModels.Lekarstva;

namespace TravelАgency.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Пользователь
            CreateMap<User, UserDb>().ReverseMap();
            CreateMap<User, LoginViewModel>().ReverseMap();
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, ConfirmEmailViewModel>().ReverseMap();
            CreateMap<User, ConfirmEmailViewModel>().ReverseMap();

            // Лекарства
            CreateMap<Lekarstva, LekarstvaDb>().ReverseMap();
            CreateMap<Lekarstva, LekarstvaViewModel>().ReverseMap();
            CreateMap<Lekarstva, LekarstvaPageViewModel>().ReverseMap();

            // Картинки лекарств
            CreateMap<PictureLekarstva, PictureLekarstvaDb>().ReverseMap();
            CreateMap<PictureLekarstva, PictureLekarstvaViewModel>().ReverseMap();

            // Заказы
            CreateMap<Zakaz, ZakazDb>().ReverseMap();
        }
    }
}