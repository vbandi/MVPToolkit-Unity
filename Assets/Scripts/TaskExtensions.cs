using System;
using System.Threading.Tasks;
using UniRx;

public static class TaskExtensions
{
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