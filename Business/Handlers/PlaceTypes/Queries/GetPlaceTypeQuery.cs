
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.PlaceTypes.Queries
{
    public class GetPlaceTypeQuery : IRequest<IDataResult<PlaceType>>
    {
        public int PlaceTypeId { get; set; }

        public class GetPlaceTypeQueryHandler : IRequestHandler<GetPlaceTypeQuery, IDataResult<PlaceType>>
        {
            private readonly IPlaceTypeRepository _placeTypeRepository;
            private readonly IMediator _mediator;

            public GetPlaceTypeQueryHandler(IPlaceTypeRepository placeTypeRepository, IMediator mediator)
            {
                _placeTypeRepository = placeTypeRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<PlaceType>> Handle(GetPlaceTypeQuery request, CancellationToken cancellationToken)
            {
                var placeType = await _placeTypeRepository.GetAsync(p => p.PlaceTypeId == request.PlaceTypeId);
                return new SuccessDataResult<PlaceType>(placeType);
            }
        }
    }
}
