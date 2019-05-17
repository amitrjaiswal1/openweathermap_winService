using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WeatherEngine.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            WeatherEngineService oWeatherEngineService = new WeatherEngineService();
            oWeatherEngineService.OnDebug();

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new WeatherEngineService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
