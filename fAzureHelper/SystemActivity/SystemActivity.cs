using System;
using System.Diagnostics;
using System.Reflection;

namespace fAzureHelper
{
    public class SystemActivity : SystemActivityEnvironment
    {
        public string AppName { get; set; }
        public string Message { get; set; }
        public DateTime UtcDateTime { get; set; }

        public SystemActivity(string message, TraceLevel type) : base()
        {
            this.UtcDateTime = DateTime.UtcNow;
            this.AppName = Assembly.GetEntryAssembly().FullName;
            this.Type = type;
            this.Message = message;
        }

        public string ToJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
        public static SystemActivity FromJson(string json)
        {
            var sa = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemActivity>(json);
            return sa;
        }
    }
}
