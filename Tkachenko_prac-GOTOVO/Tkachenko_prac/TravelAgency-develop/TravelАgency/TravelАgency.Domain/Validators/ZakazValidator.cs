using FluentValidation;
using TravelАgency.Domain.Models;

namespace TravelАgency.Domain.Validators
{
    public class ZakazValidator : AbstractValidator<Zakaz>
    {
        public ZakazValidator()
        {
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("Id пользователя обязателен");
            RuleFor(x => x.IdLekarstva).NotEmpty().WithMessage("Id лекарства обязателен");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Цена должна быть больше нуля");
        }
    }
}