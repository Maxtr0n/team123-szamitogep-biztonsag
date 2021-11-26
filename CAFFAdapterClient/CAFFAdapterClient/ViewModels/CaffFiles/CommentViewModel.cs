using System;

namespace CAFFAdapterClient.ViewModels.CaffFiles
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime CreationAt { get; set; }
        public string Message { get; set; }
    }
}
