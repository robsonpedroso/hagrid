using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Hagrid.Core.Domain.Services
{
    public class RequestInfoService : IRequestInfoService
    {
        public T GetInfoRequest<T>(string key)
        {
            if (HttpContext.Current.Items.Contains(key))
                return (T)HttpContext.Current.Items[key];

            return default(T);
        }

        public void SetInfoRequest(object key, object value)
        {
            if (!HttpContext.Current.Items.Contains(key))
                HttpContext.Current.Items.Add(key, value);
            else
                HttpContext.Current.Items[key] = value;
        }
    }
}
