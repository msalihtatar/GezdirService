
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Comments.ValidationRules;
using DataAccess.Abstract;

namespace Business.Handlers.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCommentCommand : IRequest<IResult>
    {

        public int LocationId { get; set; }
        public string CommentContent { get; set; }


        public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, IResult>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMediator _mediator;
            public CreateCommentCommandHandler(ICommentRepository commentRepository, IMediator mediator)
            {
                _commentRepository = commentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCommentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
            {
                var isThereCommentRecord = _commentRepository.Query().Any(u => u.LocationId == request.LocationId);

                if (isThereCommentRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedComment = new Comment
                {
                    LocationId = request.LocationId,
                    CommentContent = request.CommentContent,

                };

                _commentRepository.Add(addedComment);
                await _commentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}