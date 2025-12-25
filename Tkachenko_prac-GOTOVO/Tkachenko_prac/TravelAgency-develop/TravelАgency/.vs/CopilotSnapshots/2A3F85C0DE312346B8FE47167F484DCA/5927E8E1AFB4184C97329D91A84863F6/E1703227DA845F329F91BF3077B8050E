using AutoMapper;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.Filter;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.ModelsDb;
using TravelАgency.Domain.Response;
using TravelАgency.Service.Interfaces;
using TravelАgency.Domain.Enum;

namespace TravelАgency.Service.Realizations
{
    public class LekarstvaService : ILekarstvaService
    {
        private readonly IBaseStorage<LekarstvaDb> _lekarstvaStorage;
        private readonly IBaseStorage<PictureLekarstvaDb> _pictureStorage;
        private IMapper _mapper;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public LekarstvaService(IBaseStorage<LekarstvaDb> lekarstvaStorage, IBaseStorage<PictureLekarstvaDb> pictureStorage)
        {
            _lekarstvaStorage = lekarstvaStorage;
            _pictureStorage = pictureStorage;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public BaseResponse<List<Lekarstva>> GetAllLekarstva()
        {
            try
            {
                var dbItems = _lekarstvaStorage.GetAll().OrderByDescending(p => p.CreatedAt).ToList();
                var result = _mapper.Map<List<Lekarstva>>(dbItems);

                return new BaseResponse<List<Lekarstva>>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Lekarstva>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task GetById(Guid idLekarstva)
        {
            await _lekarstvaStorage.Get(idLekarstva);
        }

        public BaseResponse<List<Lekarstva>> GetLekarstvaByFilter(LekarstvaFilter filter)
        {
            try
            {
                var allItems = GetAllLekarstva().Data;

                if (filter != null && allItems != null)
                {
                    if (filter.PriceMax > 0)
                    {
                        allItems = allItems.Where(f => f.Price <= filter.PriceMax).ToList();
                    }

                    if (filter.PriceMin >= 0)
                    {
                        allItems = allItems.Where(f => f.Price >= filter.PriceMin).ToList();
                    }

                    if (filter.Categories != null && filter.Categories.Count > 0)
                    {
                        allItems = allItems.Where(f => filter.Categories.Contains(f.Category.ToString())).ToList();
                    }
                }

                return new BaseResponse<List<Lekarstva>>
                {
                    Data = allItems,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Lekarstva>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Lekarstva>> GetLekarstvaById(Guid id)
        {
            try
            {
                var itemDb = await _lekarstvaStorage.Get(id);
                var result = _mapper.Map<Lekarstva>(itemDb);

                if (result == null)
                {
                    return new BaseResponse<Lekarstva>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<Lekarstva>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Lekarstva>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<List<PictureLekarstva>> GetPicturesByIdLekarstva(Guid id)
        {
            try
            {
                var picsDb = _pictureStorage.GetAll().Where(x => x.IdLekarstva == id).ToList();
                var result = _mapper.Map<List<PictureLekarstva>>(picsDb);

                return new BaseResponse<List<PictureLekarstva>>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<PictureLekarstva>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}