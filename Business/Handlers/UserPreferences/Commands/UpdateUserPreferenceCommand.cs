
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
using Business.Handlers.UserPreferences.ValidationRules;


namespace Business.Handlers.UserPreferences.Commands
{


    public class UpdateUserPreferenceCommand : IRequest<IResult>
    {
        public int UserId { get; set; }
        public string HistoricalPlaces { get; set; }
        public string Restaurants { get; set; }
        public string Cafes { get; set; }
        public string Beaches { get; set; }
        public string Parks { get; set; }

        public class UpdateUserPreferenceCommandHandler : IRequestHandler<UpdateUserPreferenceCommand, IResult>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;

            public UpdateUserPreferenceCommandHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateUserPreferenceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateUserPreferenceCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPreferenceRecord = await _userPreferenceRepository.GetAsync(u => u.UserId == request.UserId);


                isThereUserPreferenceRecord.HistoricalPlaces = request.HistoricalPlaces;
                isThereUserPreferenceRecord.Restaurants = request.Restaurants;
                isThereUserPreferenceRecord.Cafes = request.Cafes;
                isThereUserPreferenceRecord.Beaches = request.Beaches;
                isThereUserPreferenceRecord.Parks = request.Parks;


                _userPreferenceRepository.Update(isThereUserPreferenceRecord);
                await _userPreferenceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

