
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using DataAccess.Abstract;

namespace Business.Handlers.Addresses.Queries
{

    public class GetAddressesQuery : IRequest<IDataResult<IEnumerable<Address>>>
    {
        public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, IDataResult<IEnumerable<Address>>>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;

            public GetAddressesQueryHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Address>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Address>>(await _addressRepository.GetListAsync());
            }
        }
    }
}