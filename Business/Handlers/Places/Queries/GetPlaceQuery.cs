
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Places.Queries
{
    public class GetPlaceQuery : IRequest<IDataResult<Place>>
    {
        public int PlaceId { get; set; }

        public class GetPlaceQueryHandler : IRequestHandler<GetPlaceQuery, IDataResult<Place>>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;

            public GetPlaceQueryHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Place>> Handle(GetPlaceQuery request, CancellationToken cancellationToken)
            {
                var place = await _placeRepository.GetAsync(p => p.PlaceId == request.PlaceId);
                return new SuccessDataResult<Place>(place);
            }
        }
    }
}
