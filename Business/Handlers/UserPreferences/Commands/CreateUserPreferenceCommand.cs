
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
using Business.Handlers.UserPreferences.ValidationRules;

namespace Business.Handlers.UserPreferences.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserPreferenceCommand : IRequest<IResult>
    {

        public string HistoricalPlaces { get; set; }
        public string Restaurants { get; set; }
        public string Cafes { get; set; }
        public string Beaches { get; set; }
        public string Parks { get; set; }


        public class CreateUserPreferenceCommandHandler : IRequestHandler<CreateUserPreferenceCommand, IResult>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;
            public CreateUserPreferenceCommandHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateUserPreferenceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateUserPreferenceCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPreferenceRecord = _userPreferenceRepository.Query().Any(u => u.HistoricalPlaces == request.HistoricalPlaces);

                if (isThereUserPreferenceRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedUserPreference = new UserPreference
                {
                    HistoricalPlaces = request.HistoricalPlaces,
                    Restaurants = request.Restaurants,
                    Cafes = request.Cafes,
                    Beaches = request.Beaches,
                    Parks = request.Parks,

                };

                _userPreferenceRepository.Add(addedUserPreference);
                await _userPreferenceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}