using Business.Handlers.UserPreferences.Queries;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Locations.Queries
{
    public class GetAllLocationDetailsIncludeSuggestedQuery : IRequest<IDataResult<IEnumerable<FinalLocationModel>>>
    {
        public class GetAllLocationDetailsWithSuggestedQueryHandler : IRequestHandler<GetAllLocationDetailsIncludeSuggestedQuery, IDataResult<IEnumerable<FinalLocationModel>>>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IMediator _mediator;

            public GetAllLocationDetailsWithSuggestedQueryHandler(ILocationRepository locationRepository, IMediator mediator)
            {
                _locationRepository = locationRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<FinalLocationModel>>> Handle(GetAllLocationDetailsIncludeSuggestedQuery request, CancellationToken cancellationToken)
            {
                var allLocationDetailList = await _locationRepository.GetAllLocationDetailList();

                var finalLocationModelList = new List<FinalLocationModel>();

                if (allLocationDetailList != null && allLocationDetailList.Count > 0)
                {
                    var suggestionList = await _mediator.Send(new GetSuggestionsByAprioriAlgorithmQuery());
                    if (suggestionList.Success)
                    {
                        allLocationDetailList.ForEach(location =>
                        {
                            var finalLocationModel = new FinalLocationModel()
                            {
                                PlaceId = location.PlaceId,
                                LocationId = location.LocationId,
                                Explanation = location.Explanation,
                                PlaceName = location.PlaceName,
                                PlaceTypeId = location.PlaceTypeId,
                                ScoreNum = location.ScoreNum,
                                Xcoordinate = location.Xcoordinate,
                                Ycoordinate = location.Ycoordinate,
                                IsSuggested = suggestionList.Data.Select(x=>x.PlaceId).ToList().Contains(location.PlaceId) ? true : false
                            };

                            finalLocationModelList.Add(finalLocationModel);
                        });
                    }
                }

                return new SuccessDataResult<IEnumerable<FinalLocationModel>>(finalLocationModelList);
            }
        }
    }
}
