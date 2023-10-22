using System;
using System.Threading.Tasks;

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
        catch (Exception)
        {
            // TODO: handle exception
        }
    }
}