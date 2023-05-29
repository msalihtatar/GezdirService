
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
using Business.Handlers.Transportations.ValidationRules;


namespace Business.Handlers.Transportations.Commands
{


    public class UpdateTransportationCommand : IRequest<IResult>
    {
        public int TransportationId { get; set; }
        public int LocationId { get; set; }
        public int TransportationTypeId { get; set; }
        public string Explanation { get; set; }

        public class UpdateTransportationCommandHandler : IRequestHandler<UpdateTransportationCommand, IResult>
        {
            private readonly ITransportationRepository _transportationRepository;
            private readonly IMediator _mediator;

            public UpdateTransportationCommandHandler(ITransportationRepository transportationRepository, IMediator mediator)
            {
                _transportationRepository = transportationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTransportationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTransportationCommand request, CancellationToken cancellationToken)
            {
                var isThereTransportationRecord = await _transportationRepository.GetAsync(u => u.TransportationId == request.TransportationId);


                isThereTransportationRecord.LocationId = request.LocationId;
                isThereTransportationRecord.TransportationTypeId = request.TransportationTypeId;
                isThereTransportationRecord.Explanation = request.Explanation;


                _transportationRepository.Update(isThereTransportationRecord);
                await _transportationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

