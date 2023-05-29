
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
using Business.Handlers.Locations.ValidationRules;


namespace Business.Handlers.Locations.Commands
{


    public class UpdateLocationCommand : IRequest<IResult>
    {
        public int LocationId { get; set; }
        public int PlaceId { get; set; }
        public decimal Xcoordinate { get; set; }
        public decimal Ycoordinate { get; set; }
        public System.Collections.Generic.ICollection<Address> Addresses { get; set; }
        public System.Collections.Generic.ICollection<Comment> Comments { get; set; }
        public System.Collections.Generic.ICollection<Communication> Communications { get; set; }
        public System.Collections.Generic.ICollection<CustomerPreference> CustomerPreferences { get; set; }
        public System.Collections.Generic.ICollection<Score> Scores { get; set; }
        public System.Collections.Generic.ICollection<Transportation> Transportations { get; set; }

        public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, IResult>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public UpdateLocationCommandHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateLocationValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
            {
                var isThereLocationRecord = await _locationRepository.GetAsync(u => u.LocationId == request.LocationId);


                isThereLocationRecord.PlaceId = request.PlaceId;
                isThereLocationRecord.Xcoordinate = request.Xcoordinate;
                isThereLocationRecord.Ycoordinate = request.Ycoordinate;
                //isThereLocationRecord.Addresses = request.Addresses;
                //isThereLocationRecord.Comments = request.Comments;
                //isThereLocationRecord.Communications = request.Communications;
                //isThereLocationRecord.CustomerPreferences = request.CustomerPreferences;
                //isThereLocationRecord.Scores = request.Scores;
                //isThereLocationRecord.Transportations = request.Transportations;


                _locationRepository.Update(isThereLocationRecord);
                await _locationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

