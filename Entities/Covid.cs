using System;

namespace DogukanTURHAL.Covid19.API.Entities
{
    /**
     Bu bölümde Covid entity oluşturdum bu entity sayesinde entityframeworkCore ile code first yapı kullanarak veritabanında oluşturma yapabilmeme olanak sağladı.
     */

    /**
        Bu bölümde geliştirme yapılabilir enum olarak tanımladığım şehirler için ayrı bir entity oluşturulabilir.
     */
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
    