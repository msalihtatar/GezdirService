using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class ActivityType : IEntity
{
    public int ActivityTypeId { get; set; }

    public string ActivityTypeName { get; set; }

    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();
}
