using DogukanTURHAL.Covid19.API.Entities;
using DogukanTURHAL.Covid19.API.Services;
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
        /*
         * Bu bölümde Get Post işlemlerinin controllerını oluşturdum.
         * InitializeCovid çalıştığında yeni veriler 1000ms ara ile generate edilecektir.
         * Generate edilirken bir yandan websocket üzerinden oluşturulan veriler sisteme gönderilmektedir.
         * Oluşturulan covid vakaları 100-10000 arasında değerler oluşturmaktadır ve rastgele oluşturmaktadır.
         * Yeni oluşturulan veriler db'ye kaydedilmektedir.
         * Post işleminde ise gönderdiğimiz covid'i sisteme kaydetmektedir (Test amaçlı oluşturdum)
         */

        /*
         * Refactor edilirse
         * Entityler ile direkt bağlantı yapılmaması için dtolar oluşturulur bu dtolar automapper ile mapleme işlemi gerçekleştirilir.
         * Dbcontext'te bahsettiğim gibi generic bir yapı oluşturulup scalable bir yapı geliştirilebilir.
         * Business layer oluşturarak iş ile ilgili kodlar generic repositoryden kalıtım alarak ve aynı zamanda iş hangi entity ile ilgili ise onunla ilgili interfaceler oluşturarak istediğim iş kodlarını geliştirebiliriz.
         * 
         */
        private readonly CovidService _service;
        public CovidController(CovidService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _service.SaveCovid(covid);
            return Ok(_service.GetCovidList());   
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
