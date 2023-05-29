
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Scores.Queries
{

    public class GetScoresQuery : IRequest<IDataResult<IEnumerable<Score>>>
    {
        public class GetScoresQueryHandler : IRequestHandler<GetScoresQuery, IDataResult<IEnumerable<Score>>>
        {
            private readonly IScoreRepository _scoreRepository;
            private readonly IMediator _mediator;

            public GetScoresQueryHandler(IScoreRepository scoreRepository, IMediator mediator)
            {
                _scoreRepository = scoreRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Score>>> Handle(GetScoresQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Score>>(await _scoreRepository.GetListAsync());
            }
        }
    }
}