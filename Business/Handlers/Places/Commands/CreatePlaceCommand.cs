
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
using Business.Handlers.Places.ValidationRules;

namespace Business.Handlers.Places.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatePlaceCommand : IRequest<IResult>
    {

        public string PlaceName { get; set; }
        public int PlaceTypeId { get; set; }
        public string Explanation { get; set; }
        public System.Collections.Generic.ICollection<Location> Locations { get; set; }


        public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, IResult>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;
            public CreatePlaceCommandHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreatePlaceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
            {
                var isTherePlaceRecord = _placeRepository.Query().Any(u => u.PlaceName == request.PlaceName);

                if (isTherePlaceRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedPlace = new Place
                {
                    PlaceName = request.PlaceName,
                    PlaceTypeId = request.PlaceTypeId,
                    Explanation = request.Explanation,
                    //Locations = request.Locations,
                };

                _placeRepository.Add(addedPlace);
                await _placeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}