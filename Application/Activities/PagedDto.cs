using System.Collections.Generic;

namespace Application.Activities
{
    public class PagedDto<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}