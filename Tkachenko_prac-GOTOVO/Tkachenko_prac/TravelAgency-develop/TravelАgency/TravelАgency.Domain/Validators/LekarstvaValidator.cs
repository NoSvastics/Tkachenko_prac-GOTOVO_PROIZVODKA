using FluentValidation;
using TravelАgency.Domain.Models;

namespace TravelАgency.Domain.Validators
{
    public class LekarstvaValidator : AbstractValidator<Lekarstva>
    {
        public LekarstvaValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название обязательно");
            RuleFor(x => x.Manufacturer).NotEmpty().WithMessage("Производитель обязателен");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Цена должна быть больше нуля");
            RuleFor(x => x.Category).IsInEnum().WithMessage("Неверная категория");
        }
    }
}