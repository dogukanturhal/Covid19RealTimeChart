using DogukanTURHAL.Covid19.API.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DogukanTURHAL.Covid19.API.Hubs
{
    public class CovidHub:Hub
    {
        private readonly CovidService _service;

        public CovidHub(CovidService service)
        {
            _service = service; 
        }
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _service.GetCovidList());
        }
    }
}
