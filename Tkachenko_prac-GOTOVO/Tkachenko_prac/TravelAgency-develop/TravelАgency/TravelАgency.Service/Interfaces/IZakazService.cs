using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelАgency.Domain.Models;
using TravelАgency.Domain.Response;

namespace TravelАgency.Service.Interfaces
{
    public interface IZakazService
    {
        /// <summary>
        /// Получить список заказов пользователя
        /// </summary>
        Task<BaseResponse<List<Zakaz>>> GetUserOrders(Guid userId);

        /// <summary>
        /// Получить заказ по id
        /// </summary>
        Task<BaseResponse<Zakaz>> GetById(Guid id);

        /// <summary>
        /// Создать заказ
        /// </summary>
        Task<BaseResponse<Zakaz>> Create(Zakaz model);

        /// <summary>
        /// Удалить заказ
        /// </summary>
        Task<BaseResponse<bool>> Delete(Guid id);
    }
}