using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class CustomerPreference : IEntity
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int LocationId { get; set; }

    public virtual Location Location { get; set; }
}
