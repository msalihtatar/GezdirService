
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


namespace Business.Handlers.TransportationTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTransportationTypeCommand : IRequest<IResult>
    {
        public int TransportationTypeId { get; set; }

        public class DeleteTransportationTypeCommandHandler : IRequestHandler<DeleteTransportationTypeCommand, IResult>
        {
            private readonly ITransportationTypeRepository _transportationTypeRepository;
            private readonly IMediator _mediator;

            public DeleteTransportationTypeCommandHandler(ITransportationTypeRepository transportationTypeRepository, IMediator mediator)
            {
                _transportationTypeRepository = transportationTypeRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteTransportationTypeCommand request, CancellationToken cancellationToken)
            {
                var transportationTypeToDelete = _transportationTypeRepository.Get(p => p.TransportationTypeId == request.TransportationTypeId);

                _transportationTypeRepository.Delete(transportationTypeToDelete);
                await _transportationTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

