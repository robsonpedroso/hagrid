using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class SearchResult<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public int TotalResult { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public SearchResult()
        {
            this.Results = new List<T>();
        }

        public SearchResult(IEnumerable<T> results, DTO.SearchFilter filters, int rowCount)
        {
            this.Results = new List<T>(results);
            this.Skip = !filters.Skip.IsNull() ? filters.Skip.Value : 1;
            this.Take = !filters.Take.IsNull() ? filters.Take.Value : 10;
            this.TotalResult = rowCount;
        }
    }
}
