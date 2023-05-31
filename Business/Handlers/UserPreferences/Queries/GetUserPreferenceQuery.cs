
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.UserPreferences.Queries
{
    public class GetUserPreferenceQuery : IRequest<IDataResult<UserPreference>>
    {
        public int UserId { get; set; }

        public class GetUserPreferenceQueryHandler : IRequestHandler<GetUserPreferenceQuery, IDataResult<UserPreference>>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;

            public GetUserPreferenceQueryHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPreference>> Handle(GetUserPreferenceQuery request, CancellationToken cancellationToken)
            {
                var userPreference = await _userPreferenceRepository.GetAsync(p => p.UserId == request.UserId);
                return new SuccessDataResult<UserPreference>(userPreference);
            }
        }
    }
}
