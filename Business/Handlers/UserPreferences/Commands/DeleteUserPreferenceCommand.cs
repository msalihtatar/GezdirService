
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


namespace Business.Handlers.UserPreferences.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserPreferenceCommand : IRequest<IResult>
    {
        public int UserId { get; set; }

        public class DeleteUserPreferenceCommandHandler : IRequestHandler<DeleteUserPreferenceCommand, IResult>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;

            public DeleteUserPreferenceCommandHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteUserPreferenceCommand request, CancellationToken cancellationToken)
            {
                var userPreferenceToDelete = _userPreferenceRepository.Get(p => p.UserId == request.UserId);

                _userPreferenceRepository.Delete(userPreferenceToDelete);
                await _userPreferenceRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

