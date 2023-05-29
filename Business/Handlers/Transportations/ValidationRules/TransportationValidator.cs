
using Business.Handlers.Transportations.Commands;
using FluentValidation;

namespace Business.Handlers.Transportations.ValidationRules
{

    public class CreateTransportationValidator : AbstractValidator<CreateTransportationCommand>
    {
        public CreateTransportationValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.TransportationTypeId).NotEmpty();
            RuleFor(x => x.Explanation).NotEmpty();

        }
    }
    public class UpdateTransportationValidator : AbstractValidator<UpdateTransportationCommand>
    {
        public UpdateTransportationValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.TransportationTypeId).NotEmpty();
            RuleFor(x => x.Explanation).NotEmpty();

        }
    }
}