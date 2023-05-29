
using Business.Handlers.TransportationTypes.Commands;
using FluentValidation;

namespace Business.Handlers.TransportationTypes.ValidationRules
{

    public class CreateTransportationTypeValidator : AbstractValidator<CreateTransportationTypeCommand>
    {
        public CreateTransportationTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty();
            RuleFor(x => x.Transportations).NotEmpty();

        }
    }
    public class UpdateTransportationTypeValidator : AbstractValidator<UpdateTransportationTypeCommand>
    {
        public UpdateTransportationTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty();
            RuleFor(x => x.Transportations).NotEmpty();

        }
    }
}