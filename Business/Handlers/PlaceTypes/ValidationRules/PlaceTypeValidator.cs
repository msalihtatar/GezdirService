
using Business.Handlers.PlaceTypes.Commands;
using FluentValidation;

namespace Business.Handlers.PlaceTypes.ValidationRules
{

    public class CreatePlaceTypeValidator : AbstractValidator<CreatePlaceTypeCommand>
    {
        public CreatePlaceTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty();
            RuleFor(x => x.Places).NotEmpty();

        }
    }
    public class UpdatePlaceTypeValidator : AbstractValidator<UpdatePlaceTypeCommand>
    {
        public UpdatePlaceTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty();
            RuleFor(x => x.Places).NotEmpty();

        }
    }
}