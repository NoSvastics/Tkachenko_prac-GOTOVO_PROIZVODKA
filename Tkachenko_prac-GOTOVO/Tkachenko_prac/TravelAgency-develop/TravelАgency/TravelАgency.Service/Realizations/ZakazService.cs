using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Travel¿gency.DAL.Interfaces;
using Travel¿gency.Domain.Models;
using Travel¿gency.Domain.ModelsDb;
using Travel¿gency.Domain.Response;
using Travel¿gency.Domain.Validators;
using Travel¿gency.Service.Interfaces;
using Travel¿gency.Domain.Enum;

namespace Travel¿gency.Service.Realizations
{
    public class ZakazService : IZakazService
    {
        private readonly IBaseStorage<ZakazDb> _zakazStorage;
        private IMapper _mapper;
        private ZakazValidator _validator;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public ZakazService(IBaseStorage<ZakazDb> zakazStorage)
        {
            _zakazStorage = zakazStorage;
            _validator = new ZakazValidator();
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<BaseResponse<List<Zakaz>>> GetUserOrders(Guid userId)
        {
            try
            {
                var dbItems = await _zakazStorage.GetAll()
                    .Where(x => x.IdUser == userId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                var result = _mapper.Map<List<Zakaz>>(dbItems);

                return new BaseResponse<List<Zakaz>>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Zakaz>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Zakaz>> GetById(Guid id)
        {
            try
            {
                var dbItem = await _zakazStorage.Get(id);
                if (dbItem == null)
                    return new BaseResponse<Zakaz> { Description = "«‡Í‡Á ÌÂ Ì‡È‰ÂÌ", StatusCode = StatusCode.NotFound };

                var result = _mapper.Map<Zakaz>(dbItem);
                return new BaseResponse<Zakaz> { Data = result, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Zakaz> { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<BaseResponse<Zakaz>> Create(Zakaz model)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(model);

                model.CreatedAt = DateTime.Now;
                var dbModel = _mapper.Map<ZakazDb>(model);
                await _zakazStorage.Add(dbModel);

                var result = _mapper.Map<Zakaz>(dbModel);
                return new BaseResponse<Zakaz>
                {
                    Data = result,
                    Description = "«‡Í‡Á ÒÓÁ‰‡Ì",
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errors = string.Join(";", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<Zakaz> { Description = errors, StatusCode = StatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Zakaz> { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            try
            {
                var dbItem = await _zakazStorage.Get(id);
                if (dbItem == null)
                    return new BaseResponse<bool> { Data = false, Description = "«‡Í‡Á ÌÂ Ì‡È‰ÂÌ", StatusCode = StatusCode.NotFound };

                await _zakazStorage.Delete(dbItem);
                return new BaseResponse<bool> { Data = true, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Data = false, Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }
    }
}