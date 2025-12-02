using System;
using System.IO;
using System.Text.Json;

namespace PCL_CE_Patcher.Core
{
    public class AppConfig
    {
        // 只需要记录激活时间
        public DateTime ActivatedDate { get; set; } = DateTime.MinValue;
    }

    public static class ConfigService
    {
        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PCL CE Patcher");

        private static readonly string ConfigPath = Path.Combine(AppDataPath, "config.json");
        private static readonly string LogPath = Path.Combine(AppDataPath, "latest.log");

        public static AppConfig Config { get; private set; } = new AppConfig();

        static ConfigService()
        {
            if (!Directory.Exists(AppDataPath)) Directory.CreateDirectory(AppDataPath);
            Load();
            try { File.WriteAllText(LogPath, $"[System] Patcher Started at {DateTime.Now}\n"); } catch { }
        }

        // 判断验证是否有效
        public static bool IsAuthValid()
        {
            var now = DateTime.Now;
            var authDate = Config.ActivatedDate;

            // 1. 防篡改：如果记录的时间在未来（说明用户改了系统时间或配置文件），视为无效
            if (authDate > now)
            {
                Log("[Auth] Detected future date in config. Resetting auth.");
                Config.ActivatedDate = DateTime.Now; // 重置为当前时间，迫使重新验证（或者视为无效）
                Save();
                return false;
            }

            // 2. 有效期检查：是否在 30 天内
            TimeSpan duration = now - authDate;
            if (duration.TotalDays < 30)
            {
                return true; // 有效
            }

            Log($"[Auth] Auth expired (Last: {authDate}). Requesting re-auth.");
            return false; // 过期
        }

        public static void Load()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    string json = File.ReadAllText(ConfigPath);
                    Config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                catch (Exception ex)
                {
                    Log($"Failed to load config: {ex.Message}");
                    Config = new AppConfig();
                }
            }
        }

        public static void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Log($"Failed to save config: {ex.Message}");
            }
        }

        public static void Log(string message)
        {
            string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(logLine);
            try { File.AppendAllText(LogPath, logLine + Environment.NewLine); } catch { }
        }
    }
}