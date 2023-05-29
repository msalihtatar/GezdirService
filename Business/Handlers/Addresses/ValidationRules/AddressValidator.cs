
using Business.Handlers.Addresses.Commands;
using FluentValidation;

namespace Business.Handlers.Addresses.ValidationRules
{

    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.AddressContent).NotEmpty();

        }
    }
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.Address1).NotEmpty();

        }
    }
}