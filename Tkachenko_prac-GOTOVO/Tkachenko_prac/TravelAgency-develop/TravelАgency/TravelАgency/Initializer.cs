using ForumFTI.Service.Realization;
using TravelAgency.Service.Interfaces;
using TravelАgency.DAL.Interfaces;
using TravelАgency.DAL.Storage;
using TravelАgency.Domain.ModelsDb;
using TravelАgency.Service.Interfaces;
using TravelАgency.Service.Realizations;

namespace TravelАgency
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
            services.AddScoped<IBaseStorage<LekarstvaDb>, LekarstvaStorage>();
            services.AddScoped<IBaseStorage<PictureLekarstvaDb>, PictureLekarstvaStorage>();
            services.AddScoped<IBaseStorage<ZakazDb>, ZakazStorage>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILekarstvaService, LekarstvaService>();
            services.AddScoped<IZakazService, ZakazService>();

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}