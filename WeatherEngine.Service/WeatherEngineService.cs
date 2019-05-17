using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceProcess;
using WeatherEngine.Service.Contract;
using WeatherEngine.Service.Service;
using WeatherEngine.Service.Utilities;

namespace WeatherEngine.Service
{
    partial class WeatherEngineService : ServiceBase
    {
        IWeatherEngineRepository _iWeatherEngineRepository;

        public WeatherEngineService()
        {
            InitializeComponent();
            _iWeatherEngineRepository = new WeatherEngineRepository();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            ProcessWeatherEngine();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        private void ProcessWeatherEngine()
        {
            try
            {
                DataTable dtCitiList = _iWeatherEngineRepository.GetCities(Constants.CityConfigFilePath);

                if (dtCitiList != null && dtCitiList.Rows.Count > 0)
                {
                    string strApiHostName = Constants.HostName;
                    string strApiServiceName = Constants.ServiceName;

                    foreach (DataRow row in dtCitiList.Rows)
                    {
                        string cityId = Convert.ToString(row["Id"]);
                        string cityName = Convert.ToString(row["Name"]);

                        Dictionary<string, string> dicParams = new Dictionary<string, string>();
                        dicParams.Add("appid", Constants.AppId);
                        dicParams.Add("id", cityId);

                        var task = _iWeatherEngineRepository.GetWeatherData(strApiHostName, strApiServiceName, dicParams);
                        //task.Wait(); //Uncomment this for debugging

                        string weatherResult = task.Result;

                        if (!string.IsNullOrEmpty(weatherResult))
                        {
                            bool isInfoStored = Util.LogWheatherInfo(cityId, weatherResult);
                            if (isInfoStored)
                            {
                                Util.LogIn("Weather Infomation successfully stored for city : " + cityId);
                            }
                            else
                            {
                                Util.LogIn("Weather Infomation could not be stored stored for city : " + cityId);
                            }
                        }
                        else
                        {
                            Util.LogIn("No Weather Infomation found for city : " + cityId);
                        }
                    }
                }
                else
                {
                    Util.LogIn("No configuration found for today");
                }
            }
            catch (Exception ex)
            {
                Util.LogIn(ex);
            }
        }
    }
}
