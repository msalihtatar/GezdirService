using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Location : IEntity
{
    public int LocationId { get; set; }

    public int PlaceId { get; set; }

    public decimal Xcoordinate { get; set; }

    public decimal Ycoordinate { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Communication> Communications { get; } = new List<Communication>();

    public virtual ICollection<CustomerPreference> CustomerPreferences { get; } = new List<CustomerPreference>();

    public virtual Place Place { get; set; }

    public virtual ICollection<Score> Scores { get; } = new List<Score>();

    public virtual ICollection<Transportation> Transportations { get; } = new List<Transportation>();
}
