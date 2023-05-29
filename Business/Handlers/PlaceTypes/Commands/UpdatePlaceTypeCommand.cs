
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
using Business.Handlers.PlaceTypes.ValidationRules;


namespace Business.Handlers.PlaceTypes.Commands
{
    public class UpdatePlaceTypeCommand : IRequest<IResult>
    {
        public int PlaceTypeId { get; set; }
        public string TypeName { get; set; }
        public System.Collections.Generic.ICollection<Place> Places { get; set; }

        public class UpdatePlaceTypeCommandHandler : IRequestHandler<UpdatePlaceTypeCommand, IResult>
        {
            private readonly IPlaceTypeRepository _placeTypeRepository;
            private readonly IMediator _mediator;

            public UpdatePlaceTypeCommandHandler(IPlaceTypeRepository placeTypeRepository, IMediator mediator)
            {
                _placeTypeRepository = placeTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdatePlaceTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdatePlaceTypeCommand request, CancellationToken cancellationToken)
            {
                var isTherePlaceTypeRecord = await _placeTypeRepository.GetAsync(u => u.PlaceTypeId == request.PlaceTypeId);


                isTherePlaceTypeRecord.TypeName = request.TypeName;
                //isTherePlaceTypeRecord.Places = request.Places;


                _placeTypeRepository.Update(isTherePlaceTypeRecord);
                await _placeTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

