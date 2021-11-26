using CAFFAdapterClient.ViewModels.CaffFiles;
using System.Collections.Generic;

namespace CAFFAdapterClient.ViewModels.Account
{
    public class GetUserInfoViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public IEnumerable<CaffFileRowViewModel> CaffFiles { get; set; }
    }
}
