using System;
using System.IO;

namespace WhereIs.Infrastructure
{
    public class Configuration
    {
        public string UrlRoot { get; set; }
        public string ApiKey { get; set; }
        public string Root { get; set; } = Environment.CurrentDirectory;
        public string MapPath => Path.Combine(Root, "App_Data", "Maps");
    }
}
