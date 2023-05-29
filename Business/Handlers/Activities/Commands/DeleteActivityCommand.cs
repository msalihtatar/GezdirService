
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Activities.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteActivityCommand : IRequest<IResult>
    {
        public int ActivityId { get; set; }

        public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, IResult>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IMediator _mediator;

            public DeleteActivityCommandHandler(IActivityRepository activityRepository, IMediator mediator)
            {
                _activityRepository = activityRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
            {
                var activityToDelete = _activityRepository.Get(p => p.ActivityId == request.ActivityId);

                _activityRepository.Delete(activityToDelete);
                await _activityRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

