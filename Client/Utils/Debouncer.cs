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
    
    public static async Task<TReturn?> Debounce<TInput, TReturn>(Func<TInput, TReturn> cb, TInput arg, int delayMilliseconds = 300)
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
                return cb(arg);
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Debouncer failed");
        }

        return default;
    }
    
    public static async Task<TReturn?> Debounce<TInput, TReturn>(Func<TInput, Task<TReturn>> cb, TInput arg, int delayMilliseconds = 300)
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
            await Task.Delay(delayMilliseconds, token.Token);
            return await cb(arg);
        }
        catch (TaskCanceledException)
        {
            return default;
        }
    }
}
