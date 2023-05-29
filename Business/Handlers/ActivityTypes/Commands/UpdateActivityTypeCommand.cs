
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
using Business.Handlers.ActivityTypes.ValidationRules;


namespace Business.Handlers.ActivityTypes.Commands
{


    public class UpdateActivityTypeCommand : IRequest<IResult>
    {
        public int ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; }
        public System.Collections.Generic.ICollection<Activity> Activities { get; set; }

        public class UpdateActivityTypeCommandHandler : IRequestHandler<UpdateActivityTypeCommand, IResult>
        {
            private readonly IActivityTypeRepository _activityTypeRepository;
            private readonly IMediator _mediator;

            public UpdateActivityTypeCommandHandler(IActivityTypeRepository activityTypeRepository, IMediator mediator)
            {
                _activityTypeRepository = activityTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateActivityTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateActivityTypeCommand request, CancellationToken cancellationToken)
            {
                var isThereActivityTypeRecord = await _activityTypeRepository.GetAsync(u => u.ActivityTypeId == request.ActivityTypeId);


                isThereActivityTypeRecord.ActivityTypeName = request.ActivityTypeName;
                //isThereActivityTypeRecord.Activities = request.Activities;


                _activityTypeRepository.Update(isThereActivityTypeRecord);
                await _activityTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

