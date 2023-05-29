
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

namespace Business.Handlers.Places.Queries
{

    public class GetPlacesQuery : IRequest<IDataResult<IEnumerable<Place>>>
    {
        public class GetPlacesQueryHandler : IRequestHandler<GetPlacesQuery, IDataResult<IEnumerable<Place>>>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;

            public GetPlacesQueryHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Place>>> Handle(GetPlacesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Place>>(await _placeRepository.GetListAsync());
            }
        }
    }
}