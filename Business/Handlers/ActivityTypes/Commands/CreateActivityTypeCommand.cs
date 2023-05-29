
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
using Business.Handlers.ActivityTypes.ValidationRules;

namespace Business.Handlers.ActivityTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateActivityTypeCommand : IRequest<IResult>
    {

        public string ActivityTypeName { get; set; }
        public System.Collections.Generic.ICollection<Activity> Activities { get; set; }


        public class CreateActivityTypeCommandHandler : IRequestHandler<CreateActivityTypeCommand, IResult>
        {
            private readonly IActivityTypeRepository _activityTypeRepository;
            private readonly IMediator _mediator;
            public CreateActivityTypeCommandHandler(IActivityTypeRepository activityTypeRepository, IMediator mediator)
            {
                _activityTypeRepository = activityTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateActivityTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateActivityTypeCommand request, CancellationToken cancellationToken)
            {
                var isThereActivityTypeRecord = _activityTypeRepository.Query().Any(u => u.ActivityTypeName == request.ActivityTypeName);

                if (isThereActivityTypeRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedActivityType = new ActivityType
                {
                    ActivityTypeName = request.ActivityTypeName,
                    //Activities = request.Activities,

                };

                _activityTypeRepository.Add(addedActivityType);
                await _activityTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}