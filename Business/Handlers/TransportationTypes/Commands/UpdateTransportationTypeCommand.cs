
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
using Business.Handlers.TransportationTypes.ValidationRules;


namespace Business.Handlers.TransportationTypes.Commands
{
    public class UpdateTransportationTypeCommand : IRequest<IResult>
    {
        public int TransportationTypeId { get; set; }
        public string TypeName { get; set; }
        public System.Collections.Generic.ICollection<Transportation> Transportations { get; set; }

        public class UpdateTransportationTypeCommandHandler : IRequestHandler<UpdateTransportationTypeCommand, IResult>
        {
            private readonly ITransportationTypeRepository _transportationTypeRepository;
            private readonly IMediator _mediator;

            public UpdateTransportationTypeCommandHandler(ITransportationTypeRepository transportationTypeRepository, IMediator mediator)
            {
                _transportationTypeRepository = transportationTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTransportationTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTransportationTypeCommand request, CancellationToken cancellationToken)
            {
                var isThereTransportationTypeRecord = await _transportationTypeRepository.GetAsync(u => u.TransportationTypeId == request.TransportationTypeId);


                isThereTransportationTypeRecord.TypeName = request.TypeName;
                //isThereTransportationTypeRecord.Transportations = request.Transportations;


                _transportationTypeRepository.Update(isThereTransportationTypeRecord);
                await _transportationTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

