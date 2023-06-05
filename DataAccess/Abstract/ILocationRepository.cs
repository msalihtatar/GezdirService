
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface ILocationRepository : IEntityRepository<Location>
    {
        Task<List<LocationDto>> GetLocationDetailByPlaceId(int placeId);
    }
}