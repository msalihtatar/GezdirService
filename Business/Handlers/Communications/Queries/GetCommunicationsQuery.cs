
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

namespace Business.Handlers.Communications.Queries
{

    public class GetCommunicationsQuery : IRequest<IDataResult<IEnumerable<Communication>>>
    {
        public class GetCommunicationsQueryHandler : IRequestHandler<GetCommunicationsQuery, IDataResult<IEnumerable<Communication>>>
        {
            private readonly ICommunicationRepository _communicationRepository;
            private readonly IMediator _mediator;

            public GetCommunicationsQueryHandler(ICommunicationRepository communicationRepository, IMediator mediator)
            {
                _communicationRepository = communicationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Communication>>> Handle(GetCommunicationsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Communication>>(await _communicationRepository.GetListAsync());
            }
        }
    }
}