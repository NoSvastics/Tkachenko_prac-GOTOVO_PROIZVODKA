using TravelАgency.Domain.Filter;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.Response;

namespace TravelАgency.Service.Interfaces
{
    public interface ILekarstvaService
    {
        BaseResponse<List<Lekarstva>> GetAllLekarstva();
        Task GetById(Guid idLekarstva);
        BaseResponse<List<Lekarstva>> GetLekarstvaByFilter(LekarstvaFilter filter);
        Task<BaseResponse<Lekarstva>> GetLekarstvaById(Guid id);
        BaseResponse<List<PictureLekarstva>> GetPicturesByIdLekarstva(Guid id);
    }
}