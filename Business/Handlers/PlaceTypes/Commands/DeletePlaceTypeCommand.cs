
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


namespace Business.Handlers.PlaceTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeletePlaceTypeCommand : IRequest<IResult>
    {
        public int PlaceTypeId { get; set; }

        public class DeletePlaceTypeCommandHandler : IRequestHandler<DeletePlaceTypeCommand, IResult>
        {
            private readonly IPlaceTypeRepository _placeTypeRepository;
            private readonly IMediator _mediator;

            public DeletePlaceTypeCommandHandler(IPlaceTypeRepository placeTypeRepository, IMediator mediator)
            {
                _placeTypeRepository = placeTypeRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeletePlaceTypeCommand request, CancellationToken cancellationToken)
            {
                var placeTypeToDelete = _placeTypeRepository.Get(p => p.PlaceTypeId == request.PlaceTypeId);

                _placeTypeRepository.Delete(placeTypeToDelete);
                await _placeTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

