﻿namespace Snippets5.Scheduling
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class Scheduling
    {
        public void ScheduleTask()
        {
            IEndpointInstance endpointInstance = null;
            #region ScheduleTask
            // To send a message every 5 minutes
            IBusContext busContext = endpointInstance.CreateBusContext();
            busContext.ScheduleEvery(TimeSpan.FromMinutes(5), b => b.Send(new CallLegacySystem()));

            // Name a schedule task and invoke it every 5 minutes
            busContext.ScheduleEvery(TimeSpan.FromMinutes(5), "MyCustomTask", SomeCustomMethod);

            #endregion
        }

        Task SomeCustomMethod(IBusContext busContext)
        {
            return Task.FromResult(0);
        }

    }
    class CallLegacySystem
    {
    }
}