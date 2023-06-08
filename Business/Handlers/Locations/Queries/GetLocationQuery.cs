
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Entities.Dtos;

namespace Business.Handlers.Locations.Queries
{
    public class GetLocationQuery : IRequest<IDataResult<LocationDto>>
    {
        public int LocationId { get; set; }

        public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, IDataResult<LocationDto>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetLocationQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<LocationDto>> Handle(GetLocationQuery request, CancellationToken cancellationToken)
            {
                var location = await _locationRepository.GetLocationDetailByLocationId(request.LocationId);
                return new SuccessDataResult<LocationDto>(location);
            }
        }
    }
}