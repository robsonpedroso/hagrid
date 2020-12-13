using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("search_filter")]
    public class SearchFilter
    {
        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("term")]
        public string Term { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("skip")]
        public int? Skip { get; set; }

        [JsonProperty("take")]
        public int? Take { get; set; }

        public SearchFilter()
        {
            if (this.Take.IsNull())
                this.Take = 10;

            if (this.Skip.IsNull())
                this.Skip = 0;
        }

        public SearchFilter SetRequestStore(Guid storeCode)
        {
            this.Code = storeCode;

            return this;
        }
    }

    [JsonObject("search_filter")]
    public class SearchFilterAccount : SearchFilter
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterMetadataField : SearchFilter
    {
        [JsonProperty("type")]
        public FieldType? Type { get; set; }

        [JsonProperty("json_id")]
        public string JsonId { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterResource : SearchFilter
    {
        [JsonProperty("application_code")]
        public Guid? ApplicationCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterPermission : SearchFilter
    {
        [JsonProperty("application_code")]
        public Guid? ApplicationCode { get; set; }

        [JsonProperty("role_code")]
        public Guid? RoleCode { get; set; }

        [JsonProperty("resource_code")]
        public Guid? ResourceCode { get; set; }

        [JsonProperty("resource_name")]
        public string ResourceName { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterRole : SearchFilter
    {
        [JsonProperty("store_code")]
        public Guid StoreCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterCustomerOrder : SearchFilter
    {
        [JsonProperty("customer_order_number")]
        public string CustomerOrderNumber { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterRestriction : SearchFilter
    {
        [JsonProperty("store_code")]
        public Guid StoreCode { get; set; }

        [JsonProperty("role_code")]
        public Guid? RoleCode { get; set; }
    }

    [JsonObject("search_filter")]
    public class SearchFilterStoreAccount : SearchFilter
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("store_code")]
        public Guid StoreCode { get; set; }
    }
}
