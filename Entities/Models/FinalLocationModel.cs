using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FinalLocationModel
    {
        public int LocationId { get; set; }

        public decimal Xcoordinate { get; set; }

        public decimal Ycoordinate { get; set; }

        public int PlaceId { get; set; }

        public string PlaceName { get; set; }

        public int PlaceTypeId { get; set; }

        public string Explanation { get; set; }

        public int ScoreNum { get; set; }

        public bool IsSuggested { get; set; }
    }
}
