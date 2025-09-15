using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Shared
{
    public class GridParams
    {
        private const int MaxPageSize = 100;

        private int _pageSize = 10;
        private int _page = 1;

        public int Page
        {
            get => _page <= 0 ? 1 : _page;
            set => _page = value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string SortColumn { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc"; // asc or desc

        public string? Search { get; set; }
    }

}
