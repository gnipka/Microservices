using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Microservices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ValueHolder _holder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ValueHolder holder)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
            _holder = holder;
        }

        [HttpPost, Route("create")]
        public IActionResult Create([FromBody] WeatherForecast input)
        {
            _holder.Add(input);
            _logger.LogInformation("Вызван метод создания объекта класса WeatherForecast");
            return Ok();
        }

        [HttpPut, Route("update")]
        public IActionResult Update([FromBody] WeatherForecast input)
        {
            var value = _holder.Values.FindLast(x => x.Date == input.Date);

            if (value is null)
            {
                _holder.Add(input);
                _logger.LogInformation("Вызван метод обновления объекта класса WeatherForecast. Объект не был найден, поэтому создан новый");
            }

            else
            {
                value.TemperatureC = input.TemperatureC;
                _logger.LogInformation("Вызван метод обновления объекта класса WeatherForecast. Объект найден и обновлен");
            }

            return Ok();
        }

        [HttpDelete, Route("delete")]
        public IActionResult Delete([FromBody] DateTime date1, [FromQuery] DateTime date2)
        {
            var value = _holder.Values.FindLast(x => x.Date >= date1 && x.Date <= date2 || x.Date >= date2 && x.Date <= date1);

            if (value is null)
            {
                _logger.LogInformation("Вызван метод удаления объекта класса WeatherForecast. Объект не найден.");
                return Problem($"Вы пытаетесь удалить не существующий объект. Погода с {date1} по {date2} не найдена");
            }

            _holder.Delete(value);
            _logger.LogInformation("Вызван метод удаления объекта класса WeatherForecast. Объект удален.");
            return Ok();
        }


        [HttpGet, Route("readWithCondition")]
        public IEnumerable<WeatherForecast> Get([FromBody] DateTime date1, [FromQuery] DateTime date2)
        {
            _logger.LogInformation("Вызван метод чтения экземпляров класса WeatherForecast с условием по дате. Информация возвращена пользователю.");
            return _holder.Values.FindAll(x => x.Date >= date1 && x.Date <= date2 || x.Date >= date2 && x.Date <= date1).ToArray();
        }

        [HttpGet, Route("read")]
        public IEnumerable<WeatherForecast> Read()
        {
            _logger.LogInformation("Вызван метод чтения экземпляров класса WeatherForecast. Информация возвращена пользователю.");
            return _holder.Values.ToArray();
        }
    }
}
