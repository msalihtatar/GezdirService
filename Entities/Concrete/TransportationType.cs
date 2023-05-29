using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class TransportationType : IEntity
{
    public int TransportationTypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Transportation> Transportations { get; } = new List<Transportation>();
}
