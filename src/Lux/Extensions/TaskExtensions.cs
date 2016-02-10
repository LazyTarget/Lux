using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lux.Extensions
{
    public static class TaskExtensions
    {
        public static TResult WaitForResult<TResult>(this Task<TResult> task)
        {
            task.Wait();
            var result = task.Result;
            return result;
        }

        public static TResult WaitForResult<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
        {
            task.Wait(cancellationToken);
            var result = task.Result;
            return result;
        }

        public static TResult WaitForResult<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            task.Wait(timeout);
            var result = task.Result;
            return result;
        }

        public static TResult WaitForResult<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var ms = (int) timeout.TotalMilliseconds;
            task.Wait(ms, cancellationToken);
            var result = task.Result;
            return result;
        }

    }
}
