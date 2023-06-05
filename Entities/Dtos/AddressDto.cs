
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AddressDto : IDto
    {
        public int AddressId { get; set; }

        public string AddressContent { get; set; }
    }
}
