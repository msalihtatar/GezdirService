using Business.Handlers.Places.Queries;
using Business.Helpers.SuggestionServices;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Models;
using MediatR;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPreferences.Queries
{
    public class GetSuggestionsByAprioriAlgorithmQuery : IRequest<IDataResult<List<PlaceDto>>>
    {
        public class GetSuggestionsByAprioriAlgorithmQueryHandler : IRequestHandler<GetSuggestionsByAprioriAlgorithmQuery, IDataResult<List<PlaceDto>>>
        {
            private readonly IUserPreferenceRepository _userPreferenceRepository;
            private readonly IMediator _mediator;
            private readonly ISuggestionSystem _suggestionSystem;

            public GetSuggestionsByAprioriAlgorithmQueryHandler(IUserPreferenceRepository userPreferenceRepository, IMediator mediator, ISuggestionSystem suggestionSystem)
            {
                _userPreferenceRepository = userPreferenceRepository;
                _mediator = mediator;
                _suggestionSystem = suggestionSystem;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<List<PlaceDto>>> Handle(GetSuggestionsByAprioriAlgorithmQuery request, CancellationToken cancellationToken)
            {
                //take user preferences
                var allUserPreferences = await _userPreferenceRepository.GetListAsync();

                List<List<string>> allPreferences = new List<List<string>>();
                //make preferences a two-dimensional list, each list represents a user preference
                foreach (var userPreference in allUserPreferences) 
                {
                    List<string> preferences = new List<string>();
                    
                    string restaurants = !string.IsNullOrEmpty(userPreference.Restaurants) ? userPreference.Restaurants : string.Empty;
                    preferences.AddRange(restaurants.Split(','));

                    string historicalPlaces = !string.IsNullOrEmpty(userPreference.HistoricalPlaces) ? userPreference.HistoricalPlaces : string.Empty;
                    preferences.AddRange(historicalPlaces.Split(','));

                    string cafes = !string.IsNullOrEmpty(userPreference.Cafes) ? userPreference.Cafes : string.Empty;
                    preferences.AddRange(cafes.Split(','));

                    string parks = !string.IsNullOrEmpty(userPreference.Parks) ? userPreference.Parks : string.Empty;
                    preferences.AddRange(parks.Split(','));

                    string beaches = !string.IsNullOrEmpty(userPreference.Beaches) ? userPreference.Beaches : string.Empty;
                    preferences.AddRange(beaches.Split(','));

                    allPreferences.Add(preferences);
                }

                var result = _suggestionSystem.GetAprioriAlgorithm(allPreferences);

                if (result.Success && result.Data != null)
                {
                    List<List<PlaceDto>> recommendedPlaceGroupList = new List<List<PlaceDto>>();
                    result.Data.ForEach(suggestionList =>
                    {
                        if (suggestionList.Count() > 1)
                        {
                            List<PlaceDto> recommendedPlaceList = new List<PlaceDto>();

                            suggestionList.ForEach(suggestion =>
                            {
                                int placeId = -1;
                                int.TryParse(suggestion, out placeId);
                                var result = _mediator.Send(new GetPlaceQuery { PlaceId = placeId });

                                if (result != null && result.Result != null && result.Result.Success && result.Result.Data != null)
                                {
                                    var place = result.Result.Data;

                                    recommendedPlaceList.Add(place);
                                }
                            });

                            recommendedPlaceGroupList.Add(recommendedPlaceList);
                        }
                    });

                    //var jsonResult = JsonConvert.SerializeObject(recommendedPlaceGroupList);

                    List<PlaceDto> finalPlaceDtoList = new List<PlaceDto>();
                    List<int> placeIdList = new List<int>();

                    recommendedPlaceGroupList.ForEach(suggestionList =>
                    {
                        suggestionList.ForEach(suggestion =>
                        {
                            if (!placeIdList.Contains(suggestion.PlaceId))
                            {
                                finalPlaceDtoList.Add(suggestion);
                                placeIdList.Add(suggestion.PlaceId);
                            }
                        });
                    });

                    var resultList = finalPlaceDtoList.GroupBy(p => p.PlaceTypeId).SelectMany(g => g.Take(2)).ToList();

                    return new SuccessDataResult<List<PlaceDto>>(resultList, "Done.");
                }

                return new ErrorDataResult<List<PlaceDto>>("Not Done.");
            }
        }
    }
}
