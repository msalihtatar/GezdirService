
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

namespace Business.Handlers.Addresses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAddressCommand : IRequest<IResult>
    {
        public int AddressId { get; set; }

        public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, IResult>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;

            public DeleteAddressCommandHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
            {
                var addressToDelete = _addressRepository.Get(p => p.AddressId == request.AddressId);

                _addressRepository.Delete(addressToDelete);
                await _addressRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

