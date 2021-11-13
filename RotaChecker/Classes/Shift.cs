using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Shift : WorkDuty
    {

        public bool Night { get; }
        public bool Long { get;  }
        public bool EveningFinish { get; }


        public Shift(DateTime start, DateTime end) : base(start, end)
        {

            Night = CheckNight(start, end);

            if (!Night)
            {
                if (EndTime.Hour >= 23 || EndTime.Hour < 2)
                {
                    EveningFinish = true;
                }
            }
            else
            {
                EveningFinish = false;
            }

            if (Length.TotalHours > 10)
            {
                Long = true;
            }
            else
            {
                Long = false;
            }

        }

        private bool CheckNight(DateTime start, DateTime end)
        {
            if (start.Hour >= 23)
            {
                DateTime nextDay6Am = start.Date.AddDays(1) + new TimeSpan(6, 0, 0);

                if (DateTime.Compare(nextDay6Am, end) <= 0)
                {
                    TimeSpan ts = nextDay6Am - start;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }
                else
                {
                    TimeSpan ts = end - start;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }

            }
            else if (start.Hour < 6)
            {
                DateTime sameDay6Am = start.Date + new TimeSpan(6, 0, 0);

                if (DateTime.Compare(sameDay6Am, end) <= 0)
                {
                    TimeSpan ts = sameDay6Am - start;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }
                else
                {
                    TimeSpan ts = end - start;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }
            }
            else if (end.Hour < 6)
            {
                DateTime prevDay11Pm = end.Date.AddDays(-1) + new TimeSpan(23, 0, 0);

                if (DateTime.Compare(prevDay11Pm, start) >= 0)
                {
                    TimeSpan ts = end - prevDay11Pm;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }
                else
                {
                    TimeSpan ts = end - start;
                    if (ts.TotalHours >= 3)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (start.Date.AddDays(1) == end.Date)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
