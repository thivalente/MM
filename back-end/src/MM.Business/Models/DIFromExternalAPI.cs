using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace MM.Business.Models
{
    public class DIFromExternalAPI
    {
        public DIFromExternalAPI() { }

        public DIFromExternalAPI(string date, decimal cdi, decimal selic, decimal daily_factor, decimal selic_daily, decimal cdi_daily)
        {
            this.date = date;
            this.cdi = cdi;
            this.selic = selic;
            this.daily_factor = daily_factor;
            this.selic_daily = selic_daily;
            this.cdi_daily = cdi_daily;
        }

        public string date          { get; set; }
        public decimal cdi          { get; set; }
        public decimal selic        { get; set; }
        public decimal daily_factor { get; set; }
        public decimal selic_daily  { get; set; }
        public decimal cdi_daily    { get; set; }

        public DateTime creation_date
        {
            get
            {
                var result = DateTime.MinValue;

                DateTime.TryParseExact(this.date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);

                return result;
            }
        }
    }

    public class DIFromExternalAPI_Estrutura
    {
        public List<DIFromExternalAPI> results { get; set; } = new List<DIFromExternalAPI>();
    }
}
