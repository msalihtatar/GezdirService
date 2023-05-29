using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Transportation : IEntity
{
    public int TransportationId { get; set; }

    public int LocationId { get; set; }

    public int TransportationTypeId { get; set; }

    public string Explanation { get; set; }

    public virtual Location Location { get; set; }

    public virtual TransportationType TransportationType { get; set; }
}
