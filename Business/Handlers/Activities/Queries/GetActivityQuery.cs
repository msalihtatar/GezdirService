
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Activities.Queries
{
    public class GetActivityQuery : IRequest<IDataResult<Activity>>
    {
        public int ActivityId { get; set; }

        public class GetActivityQueryHandler : IRequestHandler<GetActivityQuery, IDataResult<Activity>>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IMediator _mediator;

            public GetActivityQueryHandler(IActivityRepository activityRepository, IMediator mediator)
            {
                _activityRepository = activityRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Activity>> Handle(GetActivityQuery request, CancellationToken cancellationToken)
            {
                var activity = await _activityRepository.GetAsync(p => p.ActivityId == request.ActivityId);
                return new SuccessDataResult<Activity>(activity);
            }
        }
    }
}
