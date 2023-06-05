using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CommentDto : IDto
    {
        public int CommentId { get; set; }

        public string CommentContent { get; set; }
    }
}
