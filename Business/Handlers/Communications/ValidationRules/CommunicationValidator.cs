
using Business.Handlers.Communications.Commands;
using FluentValidation;

namespace Business.Handlers.Communications.ValidationRules
{

    public class CreateCommunicationValidator : AbstractValidator<CreateCommunicationCommand>
    {
        public CreateCommunicationValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

        }
    }
    public class UpdateCommunicationValidator : AbstractValidator<UpdateCommunicationCommand>
    {
        public UpdateCommunicationValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

        }
    }
}