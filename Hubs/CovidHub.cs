using DogukanTURHAL.Covid19.API.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DogukanTURHAL.Covid19.API.Hubs
{
    /*
     Bu bölümde SignalR kullanarak Covid vakalarını alabileceğim bir websocket oluşturdum.

     */
    public class CovidHub:Hub
    {
        private readonly CovidService _service;

        public CovidHub(CovidService service)
        {
            _service = service; 
        }
        /*
         * Tüm clientlara gönderme işlemi yapılmaktadır.
         * Daha ileri durumlarda groups kullanarak belirli clientlara istenilen verileri gönderimi yapılabilir.
         */
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _service.GetCovidList());
            
        }
    }
}
