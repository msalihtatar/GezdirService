
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
using Business.Handlers.PlaceTypes.ValidationRules;

namespace Business.Handlers.PlaceTypes.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatePlaceTypeCommand : IRequest<IResult>
    {

        public string TypeName { get; set; }
        public System.Collections.Generic.ICollection<Place> Places { get; set; }


        public class CreatePlaceTypeCommandHandler : IRequestHandler<CreatePlaceTypeCommand, IResult>
        {
            private readonly IPlaceTypeRepository _placeTypeRepository;
            private readonly IMediator _mediator;
            public CreatePlaceTypeCommandHandler(IPlaceTypeRepository placeTypeRepository, IMediator mediator)
            {
                _placeTypeRepository = placeTypeRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreatePlaceTypeValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreatePlaceTypeCommand request, CancellationToken cancellationToken)
            {
                var isTherePlaceTypeRecord = _placeTypeRepository.Query().Any(u => u.TypeName == request.TypeName);

                if (isTherePlaceTypeRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedPlaceType = new PlaceType
                {
                    TypeName = request.TypeName,
                    //Places = request.Places,

                };

                _placeTypeRepository.Add(addedPlaceType);
                await _placeTypeRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}