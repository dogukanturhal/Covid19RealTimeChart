using System.Collections.Generic;

namespace DogukanTURHAL.Covid19.API.Entities
{
    public class CovidChart
    {
        public CovidChart()
        {
            Counts = new List<int>();   
        }
        public string CovidDate { get; set; }
        public List<int> Counts { get; set; }
    }
}
