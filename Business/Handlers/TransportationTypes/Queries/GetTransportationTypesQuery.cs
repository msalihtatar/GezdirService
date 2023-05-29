
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

namespace Business.Handlers.TransportationTypes.Queries
{

    public class GetTransportationTypesQuery : IRequest<IDataResult<IEnumerable<TransportationType>>>
    {
        public class GetTransportationTypesQueryHandler : IRequestHandler<GetTransportationTypesQuery, IDataResult<IEnumerable<TransportationType>>>
        {
            private readonly ITransportationTypeRepository _transportationTypeRepository;
            private readonly IMediator _mediator;

            public GetTransportationTypesQueryHandler(ITransportationTypeRepository transportationTypeRepository, IMediator mediator)
            {
                _transportationTypeRepository = transportationTypeRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<TransportationType>>> Handle(GetTransportationTypesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<TransportationType>>(await _transportationTypeRepository.GetListAsync());
            }
        }
    }
}