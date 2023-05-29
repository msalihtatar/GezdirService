
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
using Business.Handlers.TransportationTypes.ValidationRules;

namespace Business.Handlers.TransportationTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTransportationTypeCommand : IRequest<IResult>
    {

        public string TypeName { get; set; }
        public System.Collections.Generic.ICollection<Transportation> Transportations { get; set; }


        public class CreateTransportationTypeCommandHandler : IRequestHandler<CreateTransportationTypeCommand, IResult>
        {
            private readonly ITransportationTypeRepository _transportationTypeRepository;
            private readonly IMediator _mediator;
            public CreateTransportationTypeCommandHandler(ITransportationTypeRepository transportationTypeRepository, IMediator mediator)
            {
                _transportationTypeRepository = transportationTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTransportationTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTransportationTypeCommand request, CancellationToken cancellationToken)
            {
                var isThereTransportationTypeRecord = _transportationTypeRepository.Query().Any(u => u.TypeName == request.TypeName);

                if (isThereTransportationTypeRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTransportationType = new TransportationType
                {
                    TypeName = request.TypeName,
                    //Transportations = request.Transportations,

                };

                _transportationTypeRepository.Add(addedTransportationType);
                await _transportationTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}