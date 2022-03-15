using System.Collections.Generic;

namespace Microservices
{
    public class ValueHolder
    {
        public List<WeatherForecast> Values = new List<WeatherForecast>();

        /// <summary>
        /// Добавление сущности
        /// </summary>
        /// <param name="weather"></param>
        public void Add(WeatherForecast weather)
        {
            Values.Add(weather);
        }

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="weather"></param>
        public void Delete(WeatherForecast weather)
        {
            Values.Remove(weather);
        }
    }
}
