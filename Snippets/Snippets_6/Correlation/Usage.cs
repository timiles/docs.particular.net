﻿namespace Snippets6.Correlation
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class Usage
    {
        public async Task Correlation()
        {
            IBusContext busContext = null;

            #region custom-correlationid
            SendOptions options = new SendOptions();

            options.SetCorrelationId("My custom correlation id");

            await busContext.Send(new MyRequest(),options);

            #endregion
        }

    }
}