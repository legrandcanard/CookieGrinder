using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookieGrinder.Configuration.Entities;
using Newtonsoft.Json;

namespace CookieGrinder.Configuration
{
    public class ConfiguratonManager
    {
        const string CONFIGURATION_FILE_FULLNAME = "config.json";

        public AppConfiguration GetConfiguration()
        {
            try
            {
                string configFilePath = Path.Combine(Environment.CurrentDirectory, CONFIGURATION_FILE_FULLNAME);
                if (!File.Exists(configFilePath))
                {
                    var defaultConfiguration = new AppConfiguration();

                    //Create default config file
                    using (var fs = File.Create(configFilePath))
                    using (var writer = new StreamWriter(fs))
                    {
                        writer.Write(JsonConvert.SerializeObject(defaultConfiguration));
                    }
                    return defaultConfiguration;
                }
                else
                {
                    string configFileContent = File.ReadAllText(configFilePath);
                    AppConfiguration configuration = JsonConvert.DeserializeObject<AppConfiguration>(configFileContent);

                    if (configuration == null)
                        throw new Exception("Corrupted configuration content.");

                    return configuration;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to retrieve configuration file. Using default settings.");
                Console.WriteLine(ex.Message);
                return new AppConfiguration();
            }
        }
    }
}
