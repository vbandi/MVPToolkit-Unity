using System;
using System.Threading.Tasks;
using UniRx;

public static class TaskExtensions
{
    /// <summary>
    /// Fires an async Task safely (rethrows any exception that may happen)
    /// </summary>
    /// <param name="task"></param>
    public static async void FireAndForget(this Task task)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
            // TODO: handle exception
        }
    }
}