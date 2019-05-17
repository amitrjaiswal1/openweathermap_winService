using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using WeatherEngine.Service.Contract;
using WeatherEngine.Service.Utilities;

namespace WeatherEngine.Service.Service
{
    public class WeatherEngineRepository : IWeatherEngineRepository
    {

        /// <summary>
        /// return city list table from configuration file
        /// </summary>
        /// <param name="xmlPath">city xml xonfiguration file path</param>
        /// <returns></returns>
        public DataTable GetCities(string xmlPath)
        {
            DataTable dtConfiguration = null;
            try
            {
                string strTodaysFileName = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString() + ".xml";

                string assemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string xmlConfigurationPath = assemblyFolderPath + xmlPath + strTodaysFileName;

                DataSet dsConfigurations = new DataSet();

                dsConfigurations.ReadXml(xmlConfigurationPath);

                if (dsConfigurations.Tables.Count > 0)
                {
                    dtConfiguration = dsConfigurations.Tables[0];

                    if (dtConfiguration.Rows.Count > 0)
                    {
                        dtConfiguration = dtConfiguration.AsEnumerable()
                                            .Where(row => row.Field<string>("IsImport").ToString().ToLower() == "true")
                                            .CopyToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtConfiguration;
        }

        /// <summary>
        /// returns data table from api json data
        /// </summary>
        /// <param name="apiUrl">Api url</param>
        /// <param name="apiMethodName">Api function name to append with url</param>
        /// <param name="querystringParameters">key value pair for api parameters</param>
        /// <returns></returns>
        public async Task<string> GetWeatherData(string apiUrl, string apiMethodName, Dictionary<string, string> querystringParameters)
        {
            string result = string.Empty;
            try
            {
                Uri url = new Uri(apiUrl + apiMethodName).AddQuery(querystringParameters);

                var client = new HttpClient(new HttpClientHandler()
                {
                    UseDefaultCredentials = true
                });

                client.BaseAddress = url;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializer _serializer = new JsonSerializer();
                    using (var stream = await client.GetStreamAsync(url))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            using (var json = new JsonTextReader(reader))
                            {
                                if (json == null)
                                    return null;

                                result = _serializer.Deserialize(json).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
