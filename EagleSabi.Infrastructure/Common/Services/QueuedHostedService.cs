﻿using EagleSabi.Infrastructure.Common.Abstractions.Common.Modules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EagleSabi.Infrastructure.Common.Services;

// source: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio#queued-background-tasks
public class QueuedHostedService : BackgroundService
{
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue,
        ILogger<QueuedHostedService> logger)
    {
        TaskQueue = taskQueue;
        _logger = logger;
    }

    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Queued Hosted Service is running.");

        await BackgroundProcessingAsync(stoppingToken).ConfigureAwait(false);
    }

    private async Task BackgroundProcessingAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem =
                await TaskQueue.DequeueAsync(stoppingToken).ConfigureAwait(false);

            try
            {
                await workItem(stoppingToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken).ConfigureAwait(false);
    }
}