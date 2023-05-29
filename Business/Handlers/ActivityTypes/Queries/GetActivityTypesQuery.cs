
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

namespace Business.Handlers.ActivityTypes.Queries
{

    public class GetActivityTypesQuery : IRequest<IDataResult<IEnumerable<ActivityType>>>
    {
        public class GetActivityTypesQueryHandler : IRequestHandler<GetActivityTypesQuery, IDataResult<IEnumerable<ActivityType>>>
        {
            private readonly IActivityTypeRepository _activityTypeRepository;
            private readonly IMediator _mediator;

            public GetActivityTypesQueryHandler(IActivityTypeRepository activityTypeRepository, IMediator mediator)
            {
                _activityTypeRepository = activityTypeRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ActivityType>>> Handle(GetActivityTypesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<ActivityType>>(await _activityTypeRepository.GetListAsync());
            }
        }
    }
}