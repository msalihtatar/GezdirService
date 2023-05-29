
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Abstract;

namespace Business.Handlers.Transportations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTransportationCommand : IRequest<IResult>
    {
        public int TransportationId { get; set; }

        public class DeleteTransportationCommandHandler : IRequestHandler<DeleteTransportationCommand, IResult>
        {
            private readonly ITransportationRepository _transportationRepository;
            private readonly IMediator _mediator;

            public DeleteTransportationCommandHandler(ITransportationRepository transportationRepository, IMediator mediator)
            {
                _transportationRepository = transportationRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteTransportationCommand request, CancellationToken cancellationToken)
            {
                var transportationToDelete = _transportationRepository.Get(p => p.TransportationId == request.TransportationId);

                _transportationRepository.Delete(transportationToDelete);
                await _transportationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

