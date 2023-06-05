
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
using System.Collections.Generic;

namespace Business.Handlers.Locations.Queries
{
    public class GetLocationDetailsByPlaceIdQuery : IRequest<IDataResult<List<LocationDto>>>
    {
        public int PlaceId { get; set; }

        public class GetLocationDetailsByPlaceIdQueryHandler : IRequestHandler<GetLocationDetailsByPlaceIdQuery, IDataResult<List<LocationDto>>>
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
            public async Task<IDataResult<List<LocationDto>>> Handle(GetLocationDetailsByPlaceIdQuery request, CancellationToken cancellationToken)
            {
                var locationDetailList = await _locationRepository.GetLocationDetailByPlaceId(request.PlaceId);
                return new SuccessDataResult<List<LocationDto>>(locationDetailList);
            }
        }
    }
}
