using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Utils;

using System;
using System.Threading;
using System.Threading.Tasks;

public class Debouncer
{
    private static CancellationTokenSource? token = null;

    public static async Task Debounce<TInput>(Func<TInput, Task> cb, TInput arg, int delayMilliseconds = 300)
    {
        if (token != null)
        {
            token.Cancel();
            token.Dispose();
        }

        token = new CancellationTokenSource();
        var cancelToken = token;

        try
        {
            await Task.Delay(delayMilliseconds, cancelToken.Token);

            if (!cancelToken.Token.IsCancellationRequested)
            {
                await cb(arg);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }
}
