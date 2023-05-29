
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Scores.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteScoreCommand : IRequest<IResult>
    {
        public int ScoreId { get; set; }

        public class DeleteScoreCommandHandler : IRequestHandler<DeleteScoreCommand, IResult>
        {
            private readonly IScoreRepository _scoreRepository;
            private readonly IMediator _mediator;

            public DeleteScoreCommandHandler(IScoreRepository scoreRepository, IMediator mediator)
            {
                _scoreRepository = scoreRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteScoreCommand request, CancellationToken cancellationToken)
            {
                var scoreToDelete = _scoreRepository.Get(p => p.ScoreId == request.ScoreId);

                _scoreRepository.Delete(scoreToDelete);
                await _scoreRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

