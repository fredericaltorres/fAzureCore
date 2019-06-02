using AzureServiceBusSubHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace fAzureHelper
{

    public class SystemActivityNotificationManager
    {
        public const string SystemActivityTopic = "systemactivity";
        AzurePubSubManager pub;

        public SystemActivityNotificationManager(string serviceBusConnectionString)
        {
            pub = new AzurePubSubManager(AzurePubSubManagerType.Publish, serviceBusConnectionString, SystemActivityTopic);
        }
        public async Task NotifyAsync(List<string> messages, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            foreach (var message in messages)
                await this.NotifyAsync(message, type, sendToConsole);
        }

        public string Notify(string message, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            // Wait for the call so the notification are logged in the right order
            return NotifyAsync(message, type, sendToConsole).GetAwaiter().GetResult();
        }

        public async Task<string> NotifyAsync(string message, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            var systemActivity = new SystemActivity(message, type);
            await pub.PublishAsync(systemActivity.ToJSON());
            if (sendToConsole)
                System.Console.WriteLine($"[san:{type}]{message}");
            return message;
        }
    }
}
