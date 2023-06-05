using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class PlaceDto : IDto
    {
        public int PlaceId { get; set; }

        public string PlaceName { get; set; }

        public int PlaceTypeId { get; set; }

        public string Explanation { get; set; }

        public double ScoreNum { get; set; }

        public List<int> Locations { get; set; }
    }
}
