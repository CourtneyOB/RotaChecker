﻿using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Session : Notifications
    {
        private Rota _currentRota;
        private Month _currentMonth;
        private TemplateLibrary _templateLibrary;
        private Template _currentTemplate;

        public Rota CurrentRota 
        {
            get { return _currentRota; }
            set
            {
                _currentRota = value;
                OnPropertyChanged();
            }
        }
        public Month CurrentMonth 
        { 
            get { return _currentMonth; }
            set
            {
                _currentMonth = value;
                OnPropertyChanged();
            }
        }
        public TemplateLibrary TemplateLibrary
        {
            get { return _templateLibrary; }
            set
            {
                _templateLibrary = value;
            }
        }
        public Template CurrentTemplate
        {
            get { return _currentTemplate; }
            set
            {
                _currentTemplate = value;
                OnPropertyChanged();
            }
        }

        public Session()
        {
            CurrentRota = RotaBuilder.CreateRota();
            TemplateLibrary = new TemplateLibrary();
            CurrentMonth = new Month(CurrentRota.RotaStartTime);

        }

    }
}
