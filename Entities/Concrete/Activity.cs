using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Activity : IEntity
{
    public int ActivityId { get; set; }

    public string ActivityHeader { get; set; }

    public string ActivityContent { get; set; }

    public int? ActivityTypeId { get; set; }

    public virtual ActivityType ActivityType { get; set; }
}
