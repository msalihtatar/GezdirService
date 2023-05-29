
using Business.BusinessAspects;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using DataAccess.Abstract;

namespace Business.Handlers.Comments.Queries
{
    public class GetCommentQuery : IRequest<IDataResult<Comment>>
    {
        public int CommentId { get; set; }

        public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, IDataResult<Comment>>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMediator _mediator;

            public GetCommentQueryHandler(ICommentRepository commentRepository, IMediator mediator)
            {
                _commentRepository = commentRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Comment>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetAsync(p => p.CommentId == request.CommentId);
                return new SuccessDataResult<Comment>(comment);
            }
        }
    }
}
