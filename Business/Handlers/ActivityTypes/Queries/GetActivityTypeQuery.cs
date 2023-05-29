
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.ActivityTypes.Queries
{
    public class GetActivityTypeQuery : IRequest<IDataResult<ActivityType>>
    {
        public int ActivityTypeId { get; set; }

        public class GetActivityTypeQueryHandler : IRequestHandler<GetActivityTypeQuery, IDataResult<ActivityType>>
        {
            private readonly IActivityTypeRepository _activityTypeRepository;
            private readonly IMediator _mediator;

            public GetActivityTypeQueryHandler(IActivityTypeRepository activityTypeRepository, IMediator mediator)
            {
                _activityTypeRepository = activityTypeRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ActivityType>> Handle(GetActivityTypeQuery request, CancellationToken cancellationToken)
            {
                var activityType = await _activityTypeRepository.GetAsync(p => p.ActivityTypeId == request.ActivityTypeId);
                return new SuccessDataResult<ActivityType>(activityType);
            }
        }
    }
}
