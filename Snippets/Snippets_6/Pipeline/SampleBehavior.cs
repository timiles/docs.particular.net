﻿namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SamplePipelineBehavior

    public class SampleBehavior : Behavior<LogicalMessageProcessingContext>
    {
        public override async Task Invoke(LogicalMessageProcessingContext context, Func<Task> next)
        {
            // custom logic before calling the next step in the pipeline.

            await next();

            // custom logic after all inner steps in the pipeline completed.
        }
    }

    #endregion
}