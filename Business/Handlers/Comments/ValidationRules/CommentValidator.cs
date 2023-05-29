
using Business.Handlers.Comments.Commands;
using FluentValidation;

namespace Business.Handlers.Comments.ValidationRules
{

    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.CommentContent).NotEmpty();

        }
    }
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.Comment1).NotEmpty();

        }
    }
}