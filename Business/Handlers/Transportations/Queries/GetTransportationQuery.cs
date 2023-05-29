
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Transportations.Queries
{
    public class GetTransportationQuery : IRequest<IDataResult<Transportation>>
    {
        public int TransportationId { get; set; }

        public class GetTransportationQueryHandler : IRequestHandler<GetTransportationQuery, IDataResult<Transportation>>
        {
            private readonly ITransportationRepository _transportationRepository;
            private readonly IMediator _mediator;

            public GetTransportationQueryHandler(ITransportationRepository transportationRepository, IMediator mediator)
            {
                _transportationRepository = transportationRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Transportation>> Handle(GetTransportationQuery request, CancellationToken cancellationToken)
            {
                var transportation = await _transportationRepository.GetAsync(p => p.TransportationId == request.TransportationId);
                return new SuccessDataResult<Transportation>(transportation);
            }
        }
    }
}
