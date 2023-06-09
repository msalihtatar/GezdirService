
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using System.Collections.Generic;
using Entities.Dtos;

namespace Business.Handlers.Locations.Queries
{
    public class GetLocationDetailsByPlaceIdQuery : IRequest<IDataResult<LocationDto>>
    {
        public int PlaceId { get; set; }

        public class GetLocationDetailsByPlaceIdQueryHandler : IRequestHandler<GetLocationDetailsByPlaceIdQuery, IDataResult<LocationDto>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetLocationDetailsByPlaceIdQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<LocationDto>> Handle(GetLocationDetailsByPlaceIdQuery request, CancellationToken cancellationToken)
            {
                var locationDetailList = await _locationRepository.GetLocationDetailByPlaceId(request.PlaceId);
                return new SuccessDataResult<LocationDto>(locationDetailList);
            }
        }
    }
}
