
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Scores.Queries
{
    public class GetScoreQuery : IRequest<IDataResult<Score>>
    {
        public int ScoreId { get; set; }

        public class GetScoreQueryHandler : IRequestHandler<GetScoreQuery, IDataResult<Score>>
        {
            private readonly IScoreRepository _scoreRepository;
            private readonly IMediator _mediator;

            public GetScoreQueryHandler(IScoreRepository scoreRepository, IMediator mediator)
            {
                _scoreRepository = scoreRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Score>> Handle(GetScoreQuery request, CancellationToken cancellationToken)
            {
                var score = await _scoreRepository.GetAsync(p => p.ScoreId == request.ScoreId);
                return new SuccessDataResult<Score>(score);
            }
        }
    }
}
