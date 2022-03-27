using DogukanTURHAL.Covid19.API.Entities;
using DogukanTURHAL.Covid19.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DogukanTURHAL.Covid19.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidController : ControllerBase
    {
        private readonly CovidService _service;
        public CovidController(CovidService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _service.SaveCovid(covid);
            IQueryable<Covid> covidList = _service.GetList();
            return Ok(covidList);   
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new Random();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
             {
                 foreach (ECity item in Enum.GetValues(typeof(ECity)))
                 {
                     var newCovid = new Covid { City = item, Count = rnd.Next(100, 10000), Date = DateTime.Now.AddDays(x) };
                     _service.SaveCovid(newCovid).Wait();
                     System.Threading.Thread.Sleep(1000);
                 }
             });
            return Ok("Added To Database");
        }

    }
}
