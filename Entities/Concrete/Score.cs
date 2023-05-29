using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public class Score : IEntity
{
    public int ScoreId { get; set; }

    public int LocationId { get; set; }

    public int ScoreNum { get; set; }

    public virtual Location Location { get; set; }
}
