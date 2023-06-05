
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IPlaceRepository : IEntityRepository<Place>
    {
        Task<List<PlaceDto>> GetTopPlacesDto(int topNum, int placeTypeId);
    }
}