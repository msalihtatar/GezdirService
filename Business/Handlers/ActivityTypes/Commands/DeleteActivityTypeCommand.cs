
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


namespace Business.Handlers.ActivityTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteActivityTypeCommand : IRequest<IResult>
    {
        public int ActivityTypeId { get; set; }

        public class DeleteActivityTypeCommandHandler : IRequestHandler<DeleteActivityTypeCommand, IResult>
        {
            private readonly IActivityTypeRepository _activityTypeRepository;
            private readonly IMediator _mediator;

            public DeleteActivityTypeCommandHandler(IActivityTypeRepository activityTypeRepository, IMediator mediator)
            {
                _activityTypeRepository = activityTypeRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteActivityTypeCommand request, CancellationToken cancellationToken)
            {
                var activityTypeToDelete = _activityTypeRepository.Get(p => p.ActivityTypeId == request.ActivityTypeId);

                _activityTypeRepository.Delete(activityTypeToDelete);
                await _activityTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

