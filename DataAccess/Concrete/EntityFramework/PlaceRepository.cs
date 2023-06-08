
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.Collections.Generic;
using ServiceStack;

namespace DataAccess.Concrete.EntityFramework
{
    public class PlaceRepository : EfEntityRepositoryBase<Place, ProjectDbContext>, IPlaceRepository
    {
        public PlaceRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<PlaceDto> GetPlacesDto(int placeId)
        {
            var result = await(from place in Context.Places
                               join location in Context.Locations on place.PlaceId equals location.PlaceId
                               join score in Context.Scores on location.LocationId equals score.LocationId
                               into scores
                               where place.PlaceId == placeId
                               group scores by new { place.PlaceId, place.PlaceName, place.PlaceTypeId, place.Explanation } into g
                               orderby g.SelectMany(s => s).Average(s => s.ScoreNum) descending
                               select new PlaceDto()
                               {
                                   PlaceId = g.Key.PlaceId,
                                   PlaceName = g.Key.PlaceName,
                                   PlaceTypeId = g.Key.PlaceTypeId,
                                   Explanation = g.Key.Explanation,
                                   ScoreNum = g.SelectMany(s => s).Average(s => s.ScoreNum),
                                   Locations = g.SelectMany(s => s.Select(l => l.LocationId)).ToList()
                               }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<PlaceDto>> GetTopPlacesDto(int topNum, int placeTypeId)
        {
            var result = await (from place in Context.Places
                                join location in Context.Locations on place.PlaceId equals location.PlaceId
                                join score in Context.Scores on location.LocationId equals score.LocationId 
                                into scores
                                where place.PlaceTypeId == placeTypeId
                                group scores by new { place.PlaceId, place.PlaceName, place.PlaceTypeId, place.Explanation } into g
                                orderby g.SelectMany(s => s).Average(s => s.ScoreNum) descending
                                select new PlaceDto()
                                {
                                    PlaceId = g.Key.PlaceId,
                                    PlaceName = g.Key.PlaceName,
                                    PlaceTypeId = g.Key.PlaceTypeId,
                                    Explanation = g.Key.Explanation,
                                    ScoreNum = g.SelectMany(s => s).Average(s => s.ScoreNum),
                                    Locations = g.SelectMany(s => s.Select(l => l.LocationId)).ToList()
                                }).Take(topNum).ToListAsync();

            return result;
        }
    }
}
