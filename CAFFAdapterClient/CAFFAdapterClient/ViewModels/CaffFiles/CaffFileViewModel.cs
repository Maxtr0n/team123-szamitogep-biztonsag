using System.Collections.Generic;

namespace CAFFAdapterClient.ViewModels.CaffFiles
{
    public class CaffFileViewModel
    {
        public string Description { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
