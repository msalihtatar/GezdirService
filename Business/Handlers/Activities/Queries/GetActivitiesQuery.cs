
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

namespace Business.Handlers.Activities.Queries
{

    public class GetActivitiesQuery : IRequest<IDataResult<IEnumerable<Activity>>>
    {
        public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, IDataResult<IEnumerable<Activity>>>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IMediator _mediator;

            public GetActivitiesQueryHandler(IActivityRepository activityRepository, IMediator mediator)
            {
                _activityRepository = activityRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Activity>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Activity>>(await _activityRepository.GetListAsync());
            }
        }
    }
}