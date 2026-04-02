using System;
using System.IO;

namespace TelegramService
{
    internal static class EnvLoader
    {
        public static void LoadEnv(string path = ".env")
        {
            if (!File.Exists(path)) return;
            foreach (var line in File.ReadLines(path))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;
                var idx = trimmed.IndexOf('=');
                if (idx <= 0) continue;
                var key = trimmed.Substring(0, idx).Trim();
                var value = trimmed.Substring(idx + 1).Trim();
                if (value.Length >= 2 && value.StartsWith("\"") && value.EndsWith("\""))
                    value = value[1..^1];
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}