
using Business.Handlers.Activities.Commands;
using FluentValidation;

namespace Business.Handlers.Activities.ValidationRules
{

    public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityValidator()
        {
            RuleFor(x => x.ActivityHeader).NotEmpty();
            RuleFor(x => x.ActivityContent).NotEmpty();

        }
    }
    public class UpdateActivityValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityValidator()
        {
            RuleFor(x => x.ActivityHeader).NotEmpty();
            RuleFor(x => x.ActivityContent).NotEmpty();

        }
    }
}