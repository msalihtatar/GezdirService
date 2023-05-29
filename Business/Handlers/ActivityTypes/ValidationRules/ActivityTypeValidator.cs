
using Business.Handlers.ActivityTypes.Commands;
using FluentValidation;

namespace Business.Handlers.ActivityTypes.ValidationRules
{

    public class CreateActivityTypeValidator : AbstractValidator<CreateActivityTypeCommand>
    {
        public CreateActivityTypeValidator()
        {
            RuleFor(x => x.ActivityTypeName).NotEmpty();
            RuleFor(x => x.Activities).NotEmpty();

        }
    }
    public class UpdateActivityTypeValidator : AbstractValidator<UpdateActivityTypeCommand>
    {
        public UpdateActivityTypeValidator()
        {
            RuleFor(x => x.ActivityTypeName).NotEmpty();
            RuleFor(x => x.Activities).NotEmpty();

        }
    }
}