using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Domain
{
    public class AcceptedJob
    {
        public string JobId { get; set; }

        [Column("Resource")]
        public string Controller { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime? DateFinished { get; set; }

        [Column("ResourceId")]
        public object CreatedObjectId { get; set; }

        public Guid UserId { get; set; }

        public string Payload { get; set; }

        public bool IsFinished() => DateFinished != null && CreatedObjectId != null;
    }
}
