
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Scores.ValidationRules;

namespace Business.Handlers.Scores.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateScoreCommand : IRequest<IResult>
    {

        public int LocationId { get; set; }
        public int ScoreNum { get; set; }


        public class CreateScoreCommandHandler : IRequestHandler<CreateScoreCommand, IResult>
        {
            private readonly IScoreRepository _scoreRepository;
            private readonly IMediator _mediator;
            public CreateScoreCommandHandler(IScoreRepository scoreRepository, IMediator mediator)
            {
                _scoreRepository = scoreRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateScoreValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateScoreCommand request, CancellationToken cancellationToken)
            {
                var isThereScoreRecord = _scoreRepository.Query().Any(u => u.LocationId == request.LocationId);

                if (isThereScoreRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedScore = new Score
                {
                    LocationId = request.LocationId,
                    ScoreNum = request.ScoreNum,

                };

                _scoreRepository.Add(addedScore);
                await _scoreRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}