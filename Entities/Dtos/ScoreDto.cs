using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ScoreDto : IDto
    {
        public int ScoreId { get; set; }

        public int ScoreNum { get; set; }
    }
}
