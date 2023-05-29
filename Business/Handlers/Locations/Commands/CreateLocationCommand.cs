
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
using Business.Handlers.Locations.ValidationRules;

namespace Business.Handlers.Locations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateLocationCommand : IRequest<IResult>
    {

        public int PlaceId { get; set; }
        public decimal Xcoordinate { get; set; }
        public decimal Ycoordinate { get; set; }
        public System.Collections.Generic.ICollection<Address> Addresses { get; set; }
        public System.Collections.Generic.ICollection<Comment> Comments { get; set; }
        public System.Collections.Generic.ICollection<Communication> Communications { get; set; }
        public System.Collections.Generic.ICollection<CustomerPreference> CustomerPreferences { get; set; }
        public System.Collections.Generic.ICollection<Score> Scores { get; set; }
        public System.Collections.Generic.ICollection<Transportation> Transportations { get; set; }


        public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, IResult>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;
            public CreateLocationCommandHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateLocationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
            {
                var isThereLocationRecord = _locationRepository.Query().Any(u => u.PlaceId == request.PlaceId);

                if (isThereLocationRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedLocation = new Location
                {
                    PlaceId = request.PlaceId,
                    Xcoordinate = request.Xcoordinate,
                    Ycoordinate = request.Ycoordinate,
                    //Addresses = request.Addresses,
                    //Comments = request.Comments,
                    //Communications = request.Communications,
                    //CustomerPreferences = request.CustomerPreferences,
                    //Scores = request.Scores,
                    //Transportations = request.Transportations,

                };

                _locationRepository.Add(addedLocation);
                await _locationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}