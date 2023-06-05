using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TransportationDto : IDto
    {
        public int TransportationId { get; set; }

        public int TransportationTypeId { get; set; }

        public string Explanation { get; set; }
    }
}
