
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.PlaceTypes.Queries
{

    public class GetPlaceTypesQuery : IRequest<IDataResult<IEnumerable<PlaceType>>>
    {
        public class GetPlaceTypesQueryHandler : IRequestHandler<GetPlaceTypesQuery, IDataResult<IEnumerable<PlaceType>>>
        {
            private readonly IPlaceTypeRepository _placeTypeRepository;
            private readonly IMediator _mediator;

            public GetPlaceTypesQueryHandler(IPlaceTypeRepository placeTypeRepository, IMediator mediator)
            {
                _placeTypeRepository = placeTypeRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<PlaceType>>> Handle(GetPlaceTypesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<PlaceType>>(await _placeTypeRepository.GetListAsync());
            }
        }
    }
}