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
                    case 7:
                        DisplayName = "July";
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
        public List<GridDateCell> DaysInMonth { get; set; } = new List<GridDateCell>();
        public DayOfWeek FirstDay { get; }
        public int Year { get; private set; }

        public Month(DateTime date)
        {
            MonthNumber = date.Month;
            FirstDay = new DateTime(date.Year, date.Month, 1).DayOfWeek;
            Year = date.Year;
            GetDaysInMonth(date.Month, date.Year);
        }
        public void GetDaysInMonth(int month, int year)
        {
            List<DateTime> dateList = new List<DateTime>();
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                dateList.Add(new DateTime(year, month, day));
            }

            //Work out the grid reference
            int offset = 0;
            switch (FirstDay)
            {
                case DayOfWeek.Monday:
                    offset = 0;
                    break;
                case DayOfWeek.Tuesday:
                    offset = 1;
                    break;
                case DayOfWeek.Wednesday:
                    offset = 2;
                    break;
                case DayOfWeek.Thursday:
                    offset = 3;
                    break;
                case DayOfWeek.Friday:
                    offset = 4;
                    break;
                case DayOfWeek.Saturday:
                    offset = 5;
                    break;
                case DayOfWeek.Sunday:
                    offset = 6;
                    break;
            }

            for(int i = 0; i < dateList.Count; i++)
            {
                int column;
                int row;

                if (i + offset < 7)
                {
                    row = 0;
                    column = i + offset;
                }
                else if (i + offset < 14)
                {
                    row = 1;
                    column = i + offset - 7;
                }
                else if (i + offset < 21)
                {
                    row = 2;
                    column = i + offset - 14;
                }
                else if (i + offset < 28)
                {
                    row = 3;
                    column = i + offset - 21;
                }
                else if(i + offset < 35)
                {
                    row = 4;
                    column = i + offset - 28;
                }
                else
                {
                    row = 5;
                    column = i + offset - 35;
                }

                DaysInMonth.Add(new GridDateCell(dateList[i], column, row));
            }

        }

    }
}
