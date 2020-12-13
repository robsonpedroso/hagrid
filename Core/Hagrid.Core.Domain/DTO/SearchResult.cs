using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("search_result")]
    public class SearchResult
    {
        [JsonProperty("items")]
        public IEnumerable<object> Results { get; set; }

        [JsonProperty("total_result")]
        public int TotalResult { get; set; }

        [JsonProperty("skip")]
        public int Skip { get; set; }

        [JsonProperty("take")]
        public int Take { get; set; }

        public SearchResult() { }

        public SearchResult(IEnumerable<object> results)
        {
            this.Results = new List<object>(results);
        }

        public SearchResult SetResult<T>(VO.SearchResult<T> result) where T : class
        {
            this.TotalResult = result.TotalResult;
            this.Skip = result.Skip;
            this.Take = result.Take;
            return this;
        }
    }
}
