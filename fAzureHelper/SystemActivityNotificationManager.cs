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
