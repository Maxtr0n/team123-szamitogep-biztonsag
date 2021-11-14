using System.Collections.Generic;

namespace CAFFAdapterClient.ViewModels
{
    public class TableViewModel<T>
    {
        public int Count { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
