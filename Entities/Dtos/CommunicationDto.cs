using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CommunicationDto : IDto
    {
        public int CommunicationId { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
