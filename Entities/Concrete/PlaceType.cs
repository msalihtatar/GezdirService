using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public class PlaceType : IEntity
{
    public int PlaceTypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Place> Places { get; } = new List<Place>();
}
