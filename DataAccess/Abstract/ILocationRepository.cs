using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ILocationRepository : IEntityRepository<Location>
    {
        Task<List<LocationDetailModel>> GetAllLocationDetailList();
        Task<LocationDto> GetLocationDetailByLocationId(int locationId);
        Task<List<LocationDto>> GetLocationDetailByPlaceId(int placeId);
    }
}