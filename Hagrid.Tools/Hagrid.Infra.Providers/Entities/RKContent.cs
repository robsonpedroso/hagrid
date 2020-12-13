using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.Entities
{
    public class RKContent<T>
    {
        public T content { get; set; }

        public string status { get; set; }

        public List<RKMessage> messages { get; set; }

        public string GetMessageWrapper()
        {
            return string.Join(Environment.NewLine, this.messages.Select(m => m.text));
        }

        public RKContent() { }
    }

    public class RKMessage
    {
        public string type { get; set; }

        public string text { get; set; }
    }
}
