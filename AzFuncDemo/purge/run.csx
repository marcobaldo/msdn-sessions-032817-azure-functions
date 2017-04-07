using System;
using System.Diagnostics;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    // Log a bit of information
    log.Info($"Executing purge function...");
    log.Info($"Last run {myTimer.ScheduleStatus.Last}");

    // Purge inactive records
    log.Info($"Purging inactive records...");

    var timer = new Stopwatch();
    timer.Start();

    PurgeRecords();

    log.Info($"Done in {timer.ElapsedMilliseconds}ms");
}

private static void PurgeRecords()
{
    // Do nothing! :P
}