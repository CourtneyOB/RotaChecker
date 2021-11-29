using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Session
    {

        public Rota CurrentRota { get; }
        public Month CurrentMonth { get; set; }
        public Session()
        {
            CurrentRota = RotaBuilder.CreateRota();

            CurrentMonth = new Month(CurrentRota.RotaStartTime);

        }

    }
}
