
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Communications.ValidationRules;

namespace Business.Handlers.Communications.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCommunicationCommand : IRequest<IResult>
    {

        public int LocationId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public class CreateCommunicationCommandHandler : IRequestHandler<CreateCommunicationCommand, IResult>
        {
            private readonly ICommunicationRepository _communicationRepository;
            private readonly IMediator _mediator;
            public CreateCommunicationCommandHandler(ICommunicationRepository communicationRepository, IMediator mediator)
            {
                _communicationRepository = communicationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCommunicationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCommunicationCommand request, CancellationToken cancellationToken)
            {
                var isThereCommunicationRecord = _communicationRepository.Query().Any(u => u.LocationId == request.LocationId);

                if (isThereCommunicationRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCommunication = new Communication
                {
                    LocationId = request.LocationId,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,

                };

                _communicationRepository.Add(addedCommunication);
                await _communicationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}