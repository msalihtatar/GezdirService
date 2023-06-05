using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class LocationDto : IDto
    {
        public int LocationId { get; set; }

        public decimal Xcoordinate { get; set; }

        public decimal Ycoordinate { get; set; }

        public int PlaceId { get; set; }

        public string PlaceName { get; set; }

        public int PlaceTypeId { get; set; }

        public string Explanation { get; set; }

        public int ScoreNum { get; set; }

        public string Address { get; set; } 

        public List<CommentDto> Comments { get; set; } 

        public CommunicationDto Communication { get; set; } 

        public List<TransportationDto> Transportations { get; set; }
    }
}
