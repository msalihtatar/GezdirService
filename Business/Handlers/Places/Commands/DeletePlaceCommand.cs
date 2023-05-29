
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


namespace Business.Handlers.Places.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeletePlaceCommand : IRequest<IResult>
    {
        public int PlaceId { get; set; }

        public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, IResult>
        {
            private readonly IPlaceRepository _placeRepository;
            private readonly IMediator _mediator;

            public DeletePlaceCommandHandler(IPlaceRepository placeRepository, IMediator mediator)
            {
                _placeRepository = placeRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
            {
                var placeToDelete = _placeRepository.Get(p => p.PlaceId == request.PlaceId);

                _placeRepository.Delete(placeToDelete);
                await _placeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

