using System;
using System.Collections.Generic;

namespace MM.Business.Utils
{
    public static class FeriadosHelper
    {
        public static IList<DateTime> GetHolidaysByCurrentYear(int? yearParameter = null)
        {
            var holidayList = new List<DateTime>();
            var year = DateTime.Now.Year;

            if (yearParameter != null)
                year = yearParameter.Value;

            holidayList.Add(new DateTime(year, 1, 1)); //Ano novo 
            holidayList.Add(new DateTime(year, 4, 21)); //Tiradentes
            holidayList.Add(new DateTime(year, 5, 1)); //Dia do trabalho
            holidayList.Add(new DateTime(year, 9, 7)); //Dia da Independência do Brasil
            holidayList.Add(new DateTime(year, 10, 12)); //Nossa Senhora Aparecida
            holidayList.Add(new DateTime(year, 11, 2)); //Finados
            holidayList.Add(new DateTime(year, 11, 15)); //Proclamação da República
            holidayList.Add(new DateTime(year, 12, 25)); //Natal

            #region FeriadosMóveis

            int x, y;
            int a, b, c, d, e;
            int day, month;

            if (year >= 1900 & year <= 2099)
            {
                x = 24;
                y = 5;
            }
            else if (year >= 2100 & year <= 2199)
            {
                x = 24;
                y = 6;
            }
            else if (year >= 2200 & year <= 2299)
            {
                x = 25;
                y = 7;
            }
            else
            {
                x = 24;
                y = 5;
            }

            a = year % 19;
            b = year % 4;
            c = year % 7;
            d = (19 * a + x) % 30;
            e = (2 * b + 4 * c + 6 * d + y) % 7;

            if ((d + e) > 9)
            {
                day = (d + e - 9);
                month = 4;
            }

            else
            {
                day = (d + e + 22);
                month = 3;
            }

            var pascoa = new DateTime(year, month, day);
            var sextaSanta = pascoa.AddDays(-2);
            var carnavalSegunda = pascoa.AddDays(-48);
            var carnavalTerca = pascoa.AddDays(-47);
            var corpusChristi = pascoa.AddDays(60);

            holidayList.Add(pascoa);
            holidayList.Add(sextaSanta);
            holidayList.Add(carnavalSegunda);
            holidayList.Add(carnavalTerca);
            holidayList.Add(corpusChristi);

            #endregion

            return holidayList;
        }

        public static IList<DateTime> GetWorkingDaysBetweenTwoDates(DateTime start, DateTime end)
        {
            List<DateTime> workingDays = new List<DateTime>();

            for (DateTime d = start; d <= end; d = d.AddDays(1))
            {
                if (!IsDateHolidayOrWeekend(d))
                    workingDays.Add(d);
            }

            return workingDays;
        }

        public static int GetTotalWorkingDaysInAYear(int year)
        {
            if (year < 1950)
                year = 1950;
            else if (year > 2050)
                year = 2050;

            var startD = new DateTime(year, 1, 1);
            var endD = new DateTime(year, 12, 31);

            double calcBusinessDays = 1 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            var holidays = GetHolidaysByCurrentYear(year);

            foreach (var holiday in holidays)
            {
                if (holiday.DayOfWeek != DayOfWeek.Sunday && holiday.DayOfWeek != DayOfWeek.Saturday)
                    calcBusinessDays--;
            }

            return Convert.ToInt32(calcBusinessDays);
        }

        public static bool IsDateHolidayOrWeekend(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                return true;

            var holidays = GetHolidaysByCurrentYear();

            return holidays.Contains(date);
        }

        public static bool IsTodayHolidayOrWeekend()
        {
            var today = DateTime.Today;
            return IsDateHolidayOrWeekend(today);
        }
    }
}
