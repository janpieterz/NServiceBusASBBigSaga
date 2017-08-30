using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBusBigSaga.Messages;

namespace NServiceBusBigSaga
{
    public class CreateNotification : IWantToRunWhenEndpointStartsAndStops
    {
        public Task Start(IMessageSession session)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine("How many messages would you like to send in one go?");
                    var messages = int.Parse(Console.ReadLine());
                    for (int i = 1; i <= messages; i++)
                    {
                        await session.SendLocal(new NotifyUser()
                        {
                            NotificationId = 1,
                            UserId = i
                        }).ConfigureAwait(false);
                        Console.WriteLine($"{i}/{messages} sent");
                    }
                    Console.WriteLine("Press enter to finish the notification");
                    Console.ReadLine();
                    await session.SendLocal(new FinishNotification()
                    {
                        NotificationId = 1
                    }).ConfigureAwait(false);
                }
            });
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}