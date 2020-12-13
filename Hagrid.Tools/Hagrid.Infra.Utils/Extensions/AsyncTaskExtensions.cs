using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsyncTaskExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tcs"></param>
        /// <param name="e"></param>
        /// <param name="getResult"></param>
        public static void TransferCompletion<T>(this TaskCompletionSource<T> tcs, AsyncCompletedEventArgs e, Func<T> getResult)
        {
            if (e.Error != null)
            {
                tcs.TrySetException(e.Error);
            }
            else if (e.Cancelled)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                tcs.TrySetResult(getResult());
            }
        }
    }
}
