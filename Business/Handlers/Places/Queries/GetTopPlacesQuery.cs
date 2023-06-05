
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

namespace Business.Handlers.Places.Queries
{
    public class GetTopPlacesQuery : IRequest<IDataResult<List<PlaceDto>>>
    {
        public int TopNum { get; set; }
        public int PlaceTypeId { get; set; }

        public class GetTopPlacesQueryHandler : IRequestHandler<GetTopPlacesQuery, IDataResult<List<PlaceDto>>>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;

            public GetTopPlacesQueryHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<List<PlaceDto>>> Handle(GetTopPlacesQuery request, CancellationToken cancellationToken)
            {
                var topPlaces = await _placeRepository.GetTopPlacesDto(request.TopNum, request.PlaceTypeId);
                return new SuccessDataResult<List<PlaceDto>>(topPlaces);
            }
        }
    }
}
