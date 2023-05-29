
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Abstract;

namespace Business.Handlers.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteCommentCommand : IRequest<IResult>
    {
        public int CommentId { get; set; }

        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, IResult>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMediator _mediator;

            public DeleteCommentCommandHandler(ICommentRepository commentRepository, IMediator mediator)
            {
                _commentRepository = commentRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                var commentToDelete = _commentRepository.Get(p => p.CommentId == request.CommentId);

                _commentRepository.Delete(commentToDelete);
                await _commentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

