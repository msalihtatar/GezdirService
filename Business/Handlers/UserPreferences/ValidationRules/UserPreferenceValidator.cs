
using Business.Handlers.UserPreferences.Commands;
using FluentValidation;

namespace Business.Handlers.UserPreferences.ValidationRules
{

    public class CreateUserPreferenceValidator : AbstractValidator<CreateUserPreferenceCommand>
    {
        public CreateUserPreferenceValidator()
        {
            RuleFor(x => x.HistoricalPlaces).NotEmpty();
            RuleFor(x => x.Restaurants).NotEmpty();
            RuleFor(x => x.Cafes).NotEmpty();
            RuleFor(x => x.Beaches).NotEmpty();
            RuleFor(x => x.Parks).NotEmpty();

        }
    }
    public class UpdateUserPreferenceValidator : AbstractValidator<UpdateUserPreferenceCommand>
    {
        public UpdateUserPreferenceValidator()
        {
            RuleFor(x => x.HistoricalPlaces).NotEmpty();
            RuleFor(x => x.Restaurants).NotEmpty();
            RuleFor(x => x.Cafes).NotEmpty();
            RuleFor(x => x.Beaches).NotEmpty();
            RuleFor(x => x.Parks).NotEmpty();

        }
    }
}