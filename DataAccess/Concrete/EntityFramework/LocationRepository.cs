
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using ServiceStack;

namespace DataAccess.Concrete.EntityFramework
{
    public class LocationRepository : EfEntityRepositoryBase<Location, ProjectDbContext>, ILocationRepository
    {
        public LocationRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<LocationDto>> GetLocationDetailByPlaceId(int placeId)
        {
            var locationList = await Context.Locations.Include(l => l.Place)
                                                     .Include(l => l.Scores)
                                                     .Include(l => l.Addresses)
                                                     .Include(l => l.Communications)
                                                     .Include(l => l.Transportations)
                                                     .Include(l => l.Comments)
                                                     .Where(l => l.PlaceId == placeId).Select(l => l).ToListAsync();

            List<LocationDto> locationDtoList = new List<LocationDto>(); 

            locationList.ForEach(x => 
            {
                locationDtoList.Add(new LocationDto()
                {
                    LocationId = x.LocationId,
                    Xcoordinate = x.Xcoordinate,
                    Ycoordinate = x.Ycoordinate,
                    PlaceId = x.PlaceId,
                    PlaceName = x.Place.PlaceName,
                    PlaceTypeId = x.Place.PlaceTypeId,
                    Explanation = x.Place.Explanation,
                    ScoreNum = x.Scores.Select(z => z.ScoreNum).FirstOrDefault(),
                    Address = x.Addresses.Select(a => a.AddressContent).FirstOrDefault()??string.Empty,
                    Comments = x.Comments.Select(c => new CommentDto() { CommentId = c.CommentId, CommentContent = c.CommentContent }).ToList(),
                    Transportations = x.Transportations.Select(t => new TransportationDto() { TransportationId = t.TransportationId, Explanation = t.Explanation, TransportationTypeId = t.TransportationTypeId }).ToList()
                });
            });


            return locationDtoList;
        }

        public async Task<List<LocationDetailModel>> GetAllLocationDetailList()
        {
            var locationDetailList = await Context.Locations.Include(l => l.Place)
                                                      .Include(l => l.Scores)
                                                      .Select(l => l).ToListAsync();

            List<LocationDetailModel> locationDetailModelList = new List<LocationDetailModel>();

            locationDetailList.ForEach(x =>
            {
                locationDetailModelList.Add(new LocationDetailModel()
                {
                    LocationId = x.LocationId,
                    Xcoordinate = x.Xcoordinate,
                    Ycoordinate = x.Ycoordinate,
                    PlaceId = x.PlaceId,
                    PlaceName = x.Place.PlaceName,
                    PlaceTypeId = x.Place.PlaceTypeId,
                    Explanation = x.Place.Explanation,
                    ScoreNum = x.Scores.Select(z => z.ScoreNum).FirstOrDefault()
                });
            });

            return locationDetailModelList;
        }

        public async Task<LocationDto> GetLocationDetailByLocationId(int locationId)
        {
            var locationDetail = await Context.Locations.Include(l => l.Place)
                                                        .Include(l => l.Scores)
                                                        .Include(l => l.Addresses)
                                                        .Include(l => l.Communications)
                                                        .Include(l => l.Transportations)
                                                        .Include(l => l.Comments)
                                                        .Where(l => l.LocationId == locationId)
                                                        .Select(l => l).FirstOrDefaultAsync();

            LocationDto finalLocationDetail = new LocationDto() 
            {
                LocationId = locationDetail.LocationId,
                Xcoordinate = locationDetail.Xcoordinate,
                Ycoordinate = locationDetail.Ycoordinate,
                PlaceId = locationDetail.PlaceId,
                PlaceName = locationDetail.Place.PlaceName,
                PlaceTypeId = locationDetail.Place.PlaceTypeId,
                Explanation = locationDetail.Place.Explanation,
                ScoreNum = locationDetail.Scores.Select(z => z.ScoreNum).FirstOrDefault(),
                Address = locationDetail.Addresses.Select(a => a.AddressContent).FirstOrDefault() ?? string.Empty,
                Comments = locationDetail.Comments.Select(c => new CommentDto() 
                { 
                    CommentId = c.CommentId, 
                    CommentContent = c.CommentContent 
                }).ToList(),
                Transportations = locationDetail.Transportations.Select(t => new TransportationDto() 
                { 
                    TransportationId = t.TransportationId, 
                    Explanation = t.Explanation, 
                    TransportationTypeId = t.TransportationTypeId 
                }).ToList()
            };

            return finalLocationDetail;
        }
    }
}
