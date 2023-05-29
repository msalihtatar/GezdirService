
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Communications.Queries
{
    public class GetCommunicationQuery : IRequest<IDataResult<Communication>>
    {
        public int CommunicationId { get; set; }

        public class GetCommunicationQueryHandler : IRequestHandler<GetCommunicationQuery, IDataResult<Communication>>
        {
            private readonly ICommunicationRepository _communicationRepository;
            private readonly IMediator _mediator;

            public GetCommunicationQueryHandler(ICommunicationRepository communicationRepository, IMediator mediator)
            {
                _communicationRepository = communicationRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Communication>> Handle(GetCommunicationQuery request, CancellationToken cancellationToken)
            {
                var communication = await _communicationRepository.GetAsync(p => p.CommunicationId == request.CommunicationId);
                return new SuccessDataResult<Communication>(communication);
            }
        }
    }
}
