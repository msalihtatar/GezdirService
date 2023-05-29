using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Address : IEntity
{
    public int AddressId { get; set; }

    public int LocationId { get; set; }

    public string AddressContent { get; set; }

    public virtual Location Location { get; set; }
}
