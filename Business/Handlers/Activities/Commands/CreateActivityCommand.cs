
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Activities.ValidationRules;

namespace Business.Handlers.Activities.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateActivityCommand : IRequest<IResult>
    {

        public string ActivityHeader { get; set; }
        public string ActivityContent { get; set; }
        public int? ActivityTypeId { get; set; }


        public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, IResult>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IMediator _mediator;
            public CreateActivityCommandHandler(IActivityRepository activityRepository, IMediator mediator)
            {
                _activityRepository = activityRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateActivityValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
            {
                var isThereActivityRecord = _activityRepository.Query().Any(u => u.ActivityHeader == request.ActivityHeader);

                if (isThereActivityRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedActivity = new Activity
                {
                    ActivityHeader = request.ActivityHeader,
                    ActivityContent = request.ActivityContent,
                    ActivityTypeId = request.ActivityTypeId,

                };

                _activityRepository.Add(addedActivity);
                await _activityRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}