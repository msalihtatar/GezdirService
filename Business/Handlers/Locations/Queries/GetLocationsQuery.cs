
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

namespace Business.Handlers.Locations.Queries
{

    public class GetLocationsQuery : IRequest<IDataResult<IEnumerable<Location>>>
    {
        public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IDataResult<IEnumerable<Location>>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetLocationsQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Location>>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Location>>(await _locationRepository.GetListAsync());
            }
        }
    }
}