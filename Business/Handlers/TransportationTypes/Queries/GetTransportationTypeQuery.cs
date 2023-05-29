
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.TransportationTypes.Queries
{
    public class GetTransportationTypeQuery : IRequest<IDataResult<TransportationType>>
    {
        public int TransportationTypeId { get; set; }

        public class GetTransportationTypeQueryHandler : IRequestHandler<GetTransportationTypeQuery, IDataResult<TransportationType>>
        {
            private readonly ITransportationTypeRepository _transportationTypeRepository;
            private readonly IMediator _mediator;

            public GetTransportationTypeQueryHandler(ITransportationTypeRepository transportationTypeRepository, IMediator mediator)
            {
                _transportationTypeRepository = transportationTypeRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<TransportationType>> Handle(GetTransportationTypeQuery request, CancellationToken cancellationToken)
            {
                var transportationType = await _transportationTypeRepository.GetAsync(p => p.TransportationTypeId == request.TransportationTypeId);
                return new SuccessDataResult<TransportationType>(transportationType);
            }
        }
    }
}
