
using Business.Handlers.Scores.Commands;
using FluentValidation;

namespace Business.Handlers.Scores.ValidationRules
{

    public class CreateScoreValidator : AbstractValidator<CreateScoreCommand>
    {
        public CreateScoreValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.ScoreNum).NotEmpty();

        }
    }
    public class UpdateScoreValidator : AbstractValidator<UpdateScoreCommand>
    {
        public UpdateScoreValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.ScoreNum).NotEmpty();

        }
    }
}