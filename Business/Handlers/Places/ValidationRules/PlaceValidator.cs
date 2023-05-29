
using Business.Handlers.Places.Commands;
using FluentValidation;

namespace Business.Handlers.Places.ValidationRules
{

    public class CreatePlaceValidator : AbstractValidator<CreatePlaceCommand>
    {
        public CreatePlaceValidator()
        {
            RuleFor(x => x.PlaceName).NotEmpty();
            RuleFor(x => x.PlaceTypeId).NotEmpty();
            RuleFor(x => x.Explanation).NotEmpty();
            RuleFor(x => x.Locations).NotEmpty();

        }
    }
    public class UpdatePlaceValidator : AbstractValidator<UpdatePlaceCommand>
    {
        public UpdatePlaceValidator()
        {
            RuleFor(x => x.PlaceName).NotEmpty();
            RuleFor(x => x.PlaceTypeId).NotEmpty();
            RuleFor(x => x.Explanation).NotEmpty();
            RuleFor(x => x.Locations).NotEmpty();

        }
    }
}