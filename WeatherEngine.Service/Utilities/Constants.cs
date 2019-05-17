using System;
using System.Configuration;

namespace WeatherEngine.Service.Utilities
{
    public static class Constants
    {
        public static string CityConfigFilePath { get { return ConfigurationManager.AppSettings["CityConfigFilePath"].ToString(); } }
        public static string ErrorLogFilePath { get { return ConfigurationManager.AppSettings["ErrorLogFilePath"].ToString(); } }
        public static string WeatherResultFilePath { get { return ConfigurationManager.AppSettings["WeatherResultFilePath"].ToString(); } }
        public static string HostName { get { return ConfigurationManager.AppSettings["HostName"].ToString(); } }
        public static string ServiceName { get { return ConfigurationManager.AppSettings["ServiceName"].ToString(); } }
        public static string AppId { get { return ConfigurationManager.AppSettings["AppId"].ToString(); } }
        public static double TimerInterval { get { return Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]); } }

    }
}
