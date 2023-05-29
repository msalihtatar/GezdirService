
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
using Business.Handlers.Places.ValidationRules;


namespace Business.Handlers.Places.Commands
{


    public class UpdatePlaceCommand : IRequest<IResult>
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public int PlaceTypeId { get; set; }
        public string Explanation { get; set; }
        public System.Collections.Generic.ICollection<Location> Locations { get; set; }

        public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, IResult>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;

            public UpdatePlaceCommandHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdatePlaceValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
            {
                var isTherePlaceRecord = await _placeRepository.GetAsync(u => u.PlaceId == request.PlaceId);


                isTherePlaceRecord.PlaceName = request.PlaceName;
                isTherePlaceRecord.PlaceTypeId = request.PlaceTypeId;
                isTherePlaceRecord.Explanation = request.Explanation;
                //isTherePlaceRecord.Locations = request.Locations;


                _placeRepository.Update(isTherePlaceRecord);
                await _placeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

