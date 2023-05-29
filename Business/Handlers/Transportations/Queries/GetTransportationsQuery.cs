
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

namespace Business.Handlers.Transportations.Queries
{

    public class GetTransportationsQuery : IRequest<IDataResult<IEnumerable<Transportation>>>
    {
        public class GetTransportationsQueryHandler : IRequestHandler<GetTransportationsQuery, IDataResult<IEnumerable<Transportation>>>
        {
            private readonly ITransportationRepository _transportationRepository;
            private readonly IMediator _mediator;

            public GetTransportationsQueryHandler(ITransportationRepository transportationRepository, IMediator mediator)
            {
                _transportationRepository = transportationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Transportation>>> Handle(GetTransportationsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Transportation>>(await _transportationRepository.GetListAsync());
            }
        }
    }
}