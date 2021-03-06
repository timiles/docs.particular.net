using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Callbacks.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
            Console.WriteLine("Press 'E' to send a message with an enum return");
            Console.WriteLine("Press 'I' to send a message with an int return");
            Console.WriteLine("Press 'O' to send a message with an object return");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.E)
                {
                    await SendEnumMessage(busContext);
                    continue;
                }
                if (key.Key == ConsoleKey.I)
                {
                    await SendIntMessage(busContext);
                    continue;
                }
                if (key.Key == ConsoleKey.O)
                {
                    await SendObjectMessage(busContext);
                    continue;
                }
                return;
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task SendEnumMessage(IBusContext busContext)
    {
        Console.WriteLine("Message sent");
        #region SendEnumMessage

        EnumMessage message = new EnumMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        Status status = await busContext.Request<Status>(message, sendOptions);
        Console.WriteLine("Callback received with status:" + status);
        #endregion
    }

    static async Task SendIntMessage(IBusContext busContext)
    {
        Console.WriteLine("Message sent");
        #region SendIntMessage

        IntMessage message = new IntMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        int response = await busContext.Request<int>(message, sendOptions);
        Console.WriteLine("Callback received with response:" + response);

        #endregion
    }

    static async Task SendObjectMessage(IBusContext busContext)
    {
        Console.WriteLine("Message sent");
        #region SendObjectMessage

        ObjectMessage message = new ObjectMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        ObjectResponseMessage response = await busContext.Request<ObjectResponseMessage>(message, sendOptions);
        Console.WriteLine("Callback received with response property value:" + response.Property);

        #endregion
    }
}