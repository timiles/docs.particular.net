namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

#region SharingBehaviorData

    public class ParentBehavior : Behavior<IncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IncomingPhysicalMessageContext context, Func<Task> next)
        {
            // set some shared information on the context
            context.Extensions.Set(new SharedData());

            await next();
        }
    }

    public class ChildBehavior : Behavior<IncomingLogicalMessageContext>
    {
        public override async Task Invoke(IncomingLogicalMessageContext context, Func<Task> next)
        {
            // access the shared data
            SharedData data = context.Extensions.Get<SharedData>();

            await next();
        }
    }

#endregion

    public class SharedData
    {
    }
}