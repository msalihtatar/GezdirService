
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
using Business.Handlers.Transportations.ValidationRules;

namespace Business.Handlers.Transportations.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTransportationCommand : IRequest<IResult>
    {

        public int LocationId { get; set; }
        public int TransportationTypeId { get; set; }
        public string Explanation { get; set; }


        public class CreateTransportationCommandHandler : IRequestHandler<CreateTransportationCommand, IResult>
        {
            private readonly ITransportationRepository _transportationRepository;
            private readonly IMediator _mediator;
            public CreateTransportationCommandHandler(ITransportationRepository transportationRepository, IMediator mediator)
            {
                _transportationRepository = transportationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTransportationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTransportationCommand request, CancellationToken cancellationToken)
            {
                var isThereTransportationRecord = _transportationRepository.Query().Any(u => u.LocationId == request.LocationId);

                if (isThereTransportationRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTransportation = new Transportation
                {
                    LocationId = request.LocationId,
                    TransportationTypeId = request.TransportationTypeId,
                    Explanation = request.Explanation,

                };

                _transportationRepository.Add(addedTransportation);
                await _transportationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}