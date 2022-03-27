using DogukanTURHAL.Covid19.API.Context;
using DogukanTURHAL.Covid19.API.Entities;
using DogukanTURHAL.Covid19.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogukanTURHAL.Covid19.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService (AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context; 
            _hubContext = hubContext;   
        }
        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }
        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("RecieveCovidList", GetCovidList());
        }
        public List<CovidChart> GetCovidList()
        {
            List<CovidChart> charts = new List<CovidChart>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "Select tarih, [1],[2],[3],[4],[5] FROM (Select [City], [Count],Cast([Date] as date) as tarih FROM Covids) as covidTable PIVOT (Sum(Count) For City IN([1],[2],[3],[4],[5])) as pTable order by tarih asc";
                command.CommandType = System.Data.CommandType.Text;

                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart chart = new CovidChart();

                        chart.CovidDate = reader.GetDateTime(0).ToShortDateString();

                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                         {
                             if (System.DBNull.Value.Equals(reader[x]))
                             {
                                 chart.Counts.Add(0);
                             }
                             else
                             {
                                 chart.Counts.Add(reader.GetInt32(x));
                             }
                         });
                        charts.Add(chart);
                    }
                    

                }
                _context.Database.CloseConnection();
                return charts;
            }
        }
    }
}
