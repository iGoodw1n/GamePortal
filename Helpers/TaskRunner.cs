namespace GamePortal.Helpers;

public static class TaskRunner
{
    public static async Task RunInBackground(TimeSpan timeSpan, Action action)
    {
        var periodicTimer = new PeriodicTimer(timeSpan);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            action();
        }
    }
}
