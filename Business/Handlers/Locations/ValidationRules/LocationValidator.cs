
using Business.Handlers.Locations.Commands;
using FluentValidation;

namespace Business.Handlers.Locations.ValidationRules
{

    public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.PlaceId).NotEmpty();
            RuleFor(x => x.Xcoordinate).NotEmpty();
            RuleFor(x => x.Ycoordinate).NotEmpty();
            RuleFor(x => x.Addresses).NotEmpty();
            RuleFor(x => x.Comments).NotEmpty();
            RuleFor(x => x.Communications).NotEmpty();
            RuleFor(x => x.CustomerPreferences).NotEmpty();
            RuleFor(x => x.Scores).NotEmpty();
            RuleFor(x => x.Transportations).NotEmpty();

        }
    }
    public class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationValidator()
        {
            RuleFor(x => x.PlaceId).NotEmpty();
            RuleFor(x => x.Xcoordinate).NotEmpty();
            RuleFor(x => x.Ycoordinate).NotEmpty();
            RuleFor(x => x.Addresses).NotEmpty();
            RuleFor(x => x.Comments).NotEmpty();
            RuleFor(x => x.Communications).NotEmpty();
            RuleFor(x => x.CustomerPreferences).NotEmpty();
            RuleFor(x => x.Scores).NotEmpty();
            RuleFor(x => x.Transportations).NotEmpty();

        }
    }
}