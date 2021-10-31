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
            rota.AddShift(new Shift(new DateTime(2021, 10, 10, 9, 0, 0), new DateTime(2021, 10, 10, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 11, 7, 30, 0), new DateTime(2021, 10, 11, 19, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 12, 8, 0, 0), new DateTime(2021, 10, 12, 21, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 13, 9, 0, 0), new DateTime(2021, 10, 13, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 14, 7, 0, 0), new DateTime(2021, 10, 14, 13, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 16, 12, 0, 0), new DateTime(2021, 10, 16, 20, 0, 0)));

            rota.AddShift(new Shift(new DateTime(2021, 10, 20, 12, 0, 0), new DateTime(2021, 10, 20, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 21, 12, 0, 0), new DateTime(2021, 10, 21, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 22, 12, 0, 0), new DateTime(2021, 10, 22, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 23, 12, 0, 0), new DateTime(2021, 10, 23, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 24, 12, 0, 0), new DateTime(2021, 10, 24, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 25, 12, 0, 0), new DateTime(2021, 10, 25, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 26, 12, 0, 0), new DateTime(2021, 10, 26, 20, 0, 0)));
            rota.AddShift(new Shift(new DateTime(2021, 10, 28, 9, 0, 0), new DateTime(2021, 10, 28, 20, 0, 0)));

            return rota;

        }
    }
}
