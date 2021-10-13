using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker
{
    static class RotaBuilder
    {
        static internal Rota CreateRota()
        {
            //Need validation for this - make sure shifts not overlapping, that end time is after start time etc.
            Rota rota = new Rota();
            rota.AddShift(new Shift(new DateTime(2021, 10, 10, 23, 0, 0), new DateTime(2021, 10, 11, 10, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 11, 19, 30, 0), new DateTime(2021, 10, 12, 7, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 15, 8, 0, 0), new DateTime(2021, 10, 15, 21, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 16, 23, 0, 0), new DateTime(2021, 10, 17, 10, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 12, 4, 0, 0), new DateTime(2021, 10, 12, 10, 0, 0)));

            return rota;

        }
    }
}
