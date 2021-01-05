using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Domain
{
    public class PostTag
    {
        public int Id { get; set; }

        public Guid PostId { get; set; }

        [ForeignKey("Id")]
        public virtual Tag Tag { get; set; }

        public virtual Post Post { get; set; }
    }
}
