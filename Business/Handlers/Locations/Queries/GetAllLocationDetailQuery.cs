using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Locations.Queries
{
    public class GetAllLocationDetailQuery : IRequest<IDataResult<IEnumerable<LocationDetailModel>>>
    {
        public class GetAllLocationDetailQueryHandler : IRequestHandler<GetAllLocationDetailQuery, IDataResult<IEnumerable<LocationDetailModel>>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetAllLocationDetailQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<LocationDetailModel>>> Handle(GetAllLocationDetailQuery request, CancellationToken cancellationToken)
            {
                var result = await _locationRepository.GetAllLocationDetailList();
                return new SuccessDataResult<IEnumerable<LocationDetailModel>>(result);
            }
        }
    }
}
