using System;

namespace DogukanTURHAL.Covid19.API.Entities
{
    public enum ECity
    {
        Istanbul=1,
        Ankara=2,
        Izmir=3,
        Trabzon=4,
        Antalya=5
    }
    public class Covid
    {
        public int ID { get; set; }
        public ECity City { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }
}
    