using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class Month
    {
        private int _monthNumber;

        public string DisplayName { get; private set; }
        public int MonthNumber
        {
            get { return _monthNumber; }
            set
            {
                _monthNumber = value;
                switch (MonthNumber)
                {
                    case 1:
                        DisplayName = "January";
                        break;
                    case 2:
                        DisplayName = "February";
                        break;
                    case 3:
                        DisplayName = "March";
                        break;
                    case 4:
                        DisplayName = "April";
                        break;
                    case 5:
                        DisplayName = "May";
                        break;
                    case 6:
                        DisplayName = "June";
                        break;
                    case 8:
                        DisplayName = "August";
                        break;
                    case 9:
                        DisplayName = "September";
                        break;
                    case 10:
                        DisplayName = "October";
                        break;
                    case 11:
                        DisplayName = "November";
                        break;
                    case 12:
                        DisplayName = "December";
                        break;
                }
            }
        }
        public List<DateTime> DaysInMonth { get; set; } = new List<DateTime>();
        public DayOfWeek FirstDay { get; }

        public Month(DateTime date)
        {
            MonthNumber = date.Month;
            FirstDay = new DateTime(date.Year, date.Month, 1).DayOfWeek;
            DaysInMonth = GetDaysInMonth(date.Month, date.Year);
        }
        public List<DateTime> GetDaysInMonth(int month, int year)
        {
            List<DateTime> dateList = new List<DateTime>();

            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                dateList.Add(new DateTime(year, month, day));
            }

            return dateList;
        }

    }
}
