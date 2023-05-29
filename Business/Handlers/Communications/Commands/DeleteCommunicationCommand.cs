
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


namespace Business.Handlers.Communications.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteCommunicationCommand : IRequest<IResult>
    {
        public int CommunicationId { get; set; }

        public class DeleteCommunicationCommandHandler : IRequestHandler<DeleteCommunicationCommand, IResult>
        {
            private readonly ICommunicationRepository _communicationRepository;
            private readonly IMediator _mediator;

            public DeleteCommunicationCommandHandler(ICommunicationRepository communicationRepository, IMediator mediator)
            {
                _communicationRepository = communicationRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteCommunicationCommand request, CancellationToken cancellationToken)
            {
                var communicationToDelete = _communicationRepository.Get(p => p.CommunicationId == request.CommunicationId);

                _communicationRepository.Delete(communicationToDelete);
                await _communicationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

