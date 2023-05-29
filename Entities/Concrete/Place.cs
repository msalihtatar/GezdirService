using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public class Place : IEntity
{
    public int PlaceId { get; set; }

    public string PlaceName { get; set; }

    public int PlaceTypeId { get; set; }

    public string Explanation { get; set; }

    public virtual ICollection<Location> Locations { get; } = new List<Location>();

    public virtual PlaceType PlaceType { get; set; }
}
