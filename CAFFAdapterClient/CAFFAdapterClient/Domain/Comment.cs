using System;

namespace CAFFAdapterClient.Domain
{
    public class Comment : EntityBase
    {
        public int CaffId { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public CaffFile Caff { get; set; }
    }
}
