
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.UserPreferences.Queries
{

    public class GetUserPreferencesQuery : IRequest<IDataResult<IEnumerable<UserPreference>>>
    {
        public class GetUserPreferencesQueryHandler : IRequestHandler<GetUserPreferencesQuery, IDataResult<IEnumerable<UserPreference>>>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;

            public GetUserPreferencesQueryHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserPreference>>> Handle(GetUserPreferencesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPreference>>(await _userPreferenceRepository.GetListAsync());
            }
        }
    }
}