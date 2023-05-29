
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Scores.ValidationRules;


namespace Business.Handlers.Scores.Commands
{


    public class UpdateScoreCommand : IRequest<IResult>
    {
        public int ScoreId { get; set; }
        public int LocationId { get; set; }
        public int ScoreNum { get; set; }

        public class UpdateScoreCommandHandler : IRequestHandler<UpdateScoreCommand, IResult>
        {
            private readonly IScoreRepository _scoreRepository;
            private readonly IMediator _mediator;

            public UpdateScoreCommandHandler(IScoreRepository scoreRepository, IMediator mediator)
            {
                _scoreRepository = scoreRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateScoreValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateScoreCommand request, CancellationToken cancellationToken)
            {
                var isThereScoreRecord = await _scoreRepository.GetAsync(u => u.ScoreId == request.ScoreId);


                isThereScoreRecord.LocationId = request.LocationId;
                isThereScoreRecord.ScoreNum = request.ScoreNum;


                _scoreRepository.Update(isThereScoreRecord);
                await _scoreRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

