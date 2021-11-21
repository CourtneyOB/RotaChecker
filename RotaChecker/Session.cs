using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Session
    {
        public Rota CurrentRota { get; }
        public Session()
        {
            CurrentRota = RotaBuilder.CreateRota();


        }

    }
}
