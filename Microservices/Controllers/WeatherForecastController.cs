using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _holder = holder;
        }

        [HttpPost, Route("create")]
        public IActionResult Create([FromBody] WeatherForecast input)
        {
            _holder.Add(input);

            return Ok();
        }

        [HttpPut, Route("update")]
        public IActionResult Update([FromBody] WeatherForecast input)
        {
            var value = _holder.Values.FindLast(x => x.Date == input.Date);

            if (value is null)
                _holder.Add(input);

            else
                value.TemperatureC = input.TemperatureC;

            return Ok();
        }

        [HttpDelete, Route("delete")]
        public IActionResult Delete([FromBody] DateTime date1, [FromQuery] DateTime date2)
        {
            var value = _holder.Values.FindLast(x => x.Date >= date1 && x.Date <= date2 || x.Date >= date2 && x.Date <= date1);

            if (value is null)
                return Problem($"Вы пытаетесь удалить не существующий объект. Погода с {date1} по {date2} не найдена");

            _holder.Delete(value);
            return Ok();
        }


        [HttpGet, Route("readWithCondition")]
        public IEnumerable<WeatherForecast> Get([FromBody] DateTime date1, [FromQuery] DateTime date2)
        {
            return _holder.Values.FindAll(x => x.Date >= date1 && x.Date <= date2 || x.Date >= date2 && x.Date <= date1).ToArray();
        }

        [HttpGet, Route("read")]
        public IEnumerable<WeatherForecast> Read()
        {
            return _holder.Values.ToArray();
        }
    }
}
