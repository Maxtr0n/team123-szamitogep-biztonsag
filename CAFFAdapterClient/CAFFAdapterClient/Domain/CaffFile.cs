using System;
using System.Collections.Generic;

namespace CAFFAdapterClient.Domain
{
    public class CaffFile : EntityBase
    {
        public byte[] File { get; set; }
        public byte[] Preview { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Metadata { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
