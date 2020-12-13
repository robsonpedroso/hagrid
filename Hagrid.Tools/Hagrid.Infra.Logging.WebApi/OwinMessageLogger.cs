using Microsoft.Owin;
using System.IO;
using System.Threading.Tasks;

namespace Hagrid.Infra.Logging.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class OwinMessageLogger : OwinMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public OwinMessageLogger(OwinMiddleware next) : base(next) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            var info = new MessageLoggerWebApi(context.Request);

            await info.LogRequestAsync(context.Request);

            var stream = context.Response.Body;
            var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await Next.Invoke(context);

            await info.LogResponseAsync(context.Response);

            buffer.Seek(0, SeekOrigin.Begin);
            await buffer.CopyToAsync(stream);
        }
    }
}