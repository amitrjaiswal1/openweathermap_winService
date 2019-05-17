using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace WeatherEngine.Service.Contract
{
    public interface IWeatherEngineRepository
    {
        //void GetWeatherInformationByCityId(int cityId);

        DataTable GetCities(string xmlPath);
        Task<string> GetWeatherData(string apiUrl, string apiMethodName, Dictionary<string, string> querystringParameters);

    }
}
