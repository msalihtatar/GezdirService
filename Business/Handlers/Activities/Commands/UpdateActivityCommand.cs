
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Activities.ValidationRules;


namespace Business.Handlers.Activities.Commands
{


    public class UpdateActivityCommand : IRequest<IResult>
    {
        public int ActivityId { get; set; }
        public string ActivityHeader { get; set; }
        public string ActivityContent { get; set; }
        public int? ActivityTypeId { get; set; }

        public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, IResult>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IMediator _mediator;

            public UpdateActivityCommandHandler(IActivityRepository activityRepository, IMediator mediator)
            {
                _activityRepository = activityRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateActivityValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
            {
                var isThereActivityRecord = await _activityRepository.GetAsync(u => u.ActivityId == request.ActivityId);


                isThereActivityRecord.ActivityHeader = request.ActivityHeader;
                isThereActivityRecord.ActivityContent = request.ActivityContent;
                isThereActivityRecord.ActivityTypeId = request.ActivityTypeId;


                _activityRepository.Update(isThereActivityRecord);
                await _activityRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

