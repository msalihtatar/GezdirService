
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Communications.ValidationRules;


namespace Business.Handlers.Communications.Commands
{


    public class UpdateCommunicationCommand : IRequest<IResult>
    {
        public int CommunicationId { get; set; }
        public int LocationId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public class UpdateCommunicationCommandHandler : IRequestHandler<UpdateCommunicationCommand, IResult>
        {
            private readonly ICommunicationRepository _communicationRepository;
            private readonly IMediator _mediator;

            public UpdateCommunicationCommandHandler(ICommunicationRepository communicationRepository, IMediator mediator)
            {
                _communicationRepository = communicationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCommunicationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCommunicationCommand request, CancellationToken cancellationToken)
            {
                var isThereCommunicationRecord = await _communicationRepository.GetAsync(u => u.CommunicationId == request.CommunicationId);


                isThereCommunicationRecord.LocationId = request.LocationId;
                isThereCommunicationRecord.PhoneNumber = request.PhoneNumber;
                isThereCommunicationRecord.Email = request.Email;


                _communicationRepository.Update(isThereCommunicationRecord);
                await _communicationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

