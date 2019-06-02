using System;
using System.Diagnostics;
using System.Reflection;

namespace fAzureHelper
{
    public class SystemActivity
    {
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string AppName { get; set; }
        public string AppLocation { get; set; }
        public string Message { get; set; }
        public DateTime UtcDateTime { get; set; }

        public TraceLevel Type { get; set; }


        public SystemActivity(string message, TraceLevel type)
        {
            this.UtcDateTime = DateTime.UtcNow;

            this.MachineName = Environment.MachineName;
            this.UserName = Environment.UserName;
            this.AppLocation = Assembly.GetEntryAssembly().Location;
            this.AppName = Assembly.GetEntryAssembly().FullName;

            this.Type = type;
            this.Message = message;
        }

        public string ToJSON()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}
