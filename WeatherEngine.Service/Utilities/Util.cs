using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace WeatherEngine.Service.Utilities
{
    public static class Util
    {

        /// <summary>
        /// returns uri
        /// </summary>
        /// <param name="uri">api url to call</param>
        /// <param name="urlQuerystringKeyValueParam">key value pair dictionary for querystring</param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri uri, Dictionary<string, string> urlQuerystringKeyValueParam)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            foreach (var urlParamName in urlQuerystringKeyValueParam)
            {
                httpValueCollection.Remove(urlParamName.Key.ToString());
                httpValueCollection.Add(urlParamName.Key.ToString(), urlParamName.Value);
            }

            var ub = new UriBuilder(uri);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
            if (httpValueCollection.Count == 0)
                ub.Query = string.Empty;
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++)
                {
                    string text = httpValueCollection.GetKey(i);
                    {
                        text = HttpUtility.UrlEncode(text);

                        string val = (text != null) ? (text + "=") : string.Empty;
                        string[] vals = httpValueCollection.GetValues(i);

                        if (sb.Length > 0)
                            sb.Append('&');

                        if (vals == null || vals.Length == 0)
                            sb.Append(val);
                        else
                        {
                            if (vals.Length == 1)
                            {
                                sb.Append(val);
                                sb.Append(HttpUtility.UrlEncode(vals[0]));
                            }
                            else
                            {
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    if (j > 0)
                                        sb.Append('&');

                                    sb.Append(val);
                                    sb.Append(HttpUtility.UrlEncode(vals[j]));
                                }
                            }
                        }
                    }
                }

                ub.Query = sb.ToString();
            }

            return ub.Uri;
        }

        /// <summary>
        /// returns uri
        /// </summary>
        /// <param name="uri">api url to call</param>
        /// <param name="name">querystring parameter name</param>
        /// <param name="value">querystring parameter value</param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
            if (httpValueCollection.Count == 0)
                ub.Query = String.Empty;
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++)
                {
                    string text = httpValueCollection.GetKey(i);
                    {
                        text = HttpUtility.UrlEncode(text);

                        string val = (text != null) ? (text + "=") : string.Empty;
                        string[] vals = httpValueCollection.GetValues(i);

                        if (sb.Length > 0)
                            sb.Append('&');

                        if (vals == null || vals.Length == 0)
                            sb.Append(val);
                        else
                        {
                            if (vals.Length == 1)
                            {
                                sb.Append(val);
                                sb.Append(HttpUtility.UrlEncode(vals[0]));
                            }
                            else
                            {
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    if (j > 0)
                                        sb.Append('&');

                                    sb.Append(val);
                                    sb.Append(HttpUtility.UrlEncode(vals[j]));
                                }
                            }
                        }
                    }
                }

                ub.Query = sb.ToString();
            }

            return ub.Uri;
        }


        public static void LogIn(dynamic info)
        {
            try
            {
                StringBuilder strLogFilePath = new StringBuilder();
                StringBuilder strLogInfo = new StringBuilder();

                strLogFilePath.Remove(0, strLogFilePath.Length);
                strLogInfo.Remove(0, strLogInfo.Length);

                if (info is string)
                {
                    string ex = info as string;
                    strLogInfo.Append("Information - " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + " - " + ex);
                }
                else if (info is Exception)
                {
                    Exception ex = info as Exception;
                    strLogInfo.Append("Exception - " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + " - " + ex.Message.Trim() + " : Method Name - " + ex.TargetSite.ToString().Trim() + " : Class Name - " + ex.TargetSite.DeclaringType.Name.Trim() + " : Stack Trace - " + ex.StackTrace.Trim());
                }

                strLogFilePath.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Constants.ErrorLogFilePath + DateTime.Now.ToString("ddMMMyyyy") + ".log");
                if (!File.Exists(strLogFilePath.ToString()))
                {
                    using (StreamWriter sw = File.CreateText(strLogFilePath.ToString()))
                    {
                        sw.WriteLine(strLogInfo);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(strLogFilePath.ToString()))
                    {
                        sw.WriteLine(strLogInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "DRS";
                        eventLog.WriteEntry("DRS:LogIn: " + ex.Message + ex.StackTrace, EventLogEntryType.Error);
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// store json weather infomration in text file
        /// </summary>
        /// <param name="cityId">openweather api city id</param>
        /// <param name="weatherInfo">openweather retrived city weather information</param>
        public static bool LogWheatherInfo(string cityId, string weatherInfo)
        {
            try
            {
                string strTodaysFileName = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();
                string assemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string weatherInfoDateFolderPath = assemblyFolderPath + Constants.WeatherResultFilePath + strTodaysFileName + "\\";
                string weatherInfoCityFilePath = assemblyFolderPath + Constants.WeatherResultFilePath + "\\" + strTodaysFileName + "\\" + cityId + ".txt";

                if (!Directory.Exists(weatherInfoDateFolderPath))
                {
                    Directory.CreateDirectory(weatherInfoDateFolderPath);
                }

                StringBuilder strLogFilePath = new StringBuilder();
                StringBuilder strLogInfo = new StringBuilder();

                strLogFilePath.Remove(0, strLogFilePath.Length);
                strLogInfo.Remove(0, strLogInfo.Length);

                if (!string.IsNullOrEmpty(weatherInfo))
                {
                    strLogInfo.Append(weatherInfo);

                    strLogFilePath.Append(weatherInfoCityFilePath);

                    if (!File.Exists(strLogFilePath.ToString()))
                    {
                        using (StreamWriter sw = File.CreateText(strLogFilePath.ToString()))
                        {
                            sw.WriteLine(strLogInfo);
                            return true;
                        }
                    }
                    else
                    {
                        //StreamWriter sw = File.WriteAllText(strLogFilePath.ToString(),strLogInfo.ToString());

                        File.WriteAllText(strLogFilePath.ToString(), strLogInfo.ToString());
                        return true;
                        //using (StreamWriter sw = File.WriteAllLines(strLogFilePath.ToString(), strLogInfo))
                        //{
                        //    sw.WriteLine(strLogInfo);
                        //    return true;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogIn(ex);
                return false;
            }
            return false;
        }
    }
}
