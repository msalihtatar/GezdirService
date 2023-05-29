using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete;

public partial class Comment : IEntity
{
    public int CommentId { get; set; }

    public int LocationId { get; set; }

    public string CommentContent { get; set; }

    public virtual Location Location { get; set; }
}
