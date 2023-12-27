namespace GamePortal.Helpers;

public static class TaskRunner
{
    public static async Task RunInBackground(TimeSpan timeSpan, Action action, CancellationToken token)
    {
        var periodicTimer = new PeriodicTimer(timeSpan);
        while (await periodicTimer.WaitForNextTickAsync() && !token.IsCancellationRequested)
        {
            action();
        }
    }
}
