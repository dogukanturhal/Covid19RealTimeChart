using System.Collections.Generic;

namespace DogukanTURHAL.Covid19.API.Entities
{

    /***
     * Bu bölümde complex bir entity yapısı oluşturdum
     * Bu entity covid vakalarını pivot tablo üzerinde gösterebilmek için oluşturdum.
     * 
     */
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
