using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Communication : IEntity
{
    public int CommunicationId { get; set; }

    public int LocationId { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public virtual Location Location { get; set; }
}
