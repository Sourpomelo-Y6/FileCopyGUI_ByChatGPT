using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace FileCopyGUI.Model
{
    public class ConfigService
    {
        private readonly string _configPath = "config.json";

        public Config LoadConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    var config = JsonConvert.DeserializeObject<Config>(json);

                    if (config == null)
                        return new Config();

                    return config;
                }
                catch
                {
                    // ファイルが存在するが、読み込みまたはデシリアライズに失敗した場合
                    return new Config();
                }
            }
            return new Config();
        }

        public void SaveConfig(Config config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
    }

}
