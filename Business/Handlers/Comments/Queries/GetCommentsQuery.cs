
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using DataAccess.Abstract;

namespace Business.Handlers.Comments.Queries
{

    public class GetCommentsQuery : IRequest<IDataResult<IEnumerable<Comment>>>
    {
        public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, IDataResult<IEnumerable<Comment>>>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMediator _mediator;

            public GetCommentsQueryHandler(ICommentRepository commentRepository, IMediator mediator)
            {
                _commentRepository = commentRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Comment>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Comment>>(await _commentRepository.GetListAsync());
            }
        }
    }
}