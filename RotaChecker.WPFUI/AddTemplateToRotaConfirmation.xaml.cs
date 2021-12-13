using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RotaChecker.Classes;
using System.Linq;

namespace RotaChecker.WPFUI
{
    public partial class AddTemplateToRotaConfirmation : Window
    {
        private readonly List<DateTime> _selectedDates = new List<DateTime>();
        public Session Session => DataContext as Session;
        public AddTemplateToRotaConfirmation(List<DateTime> selectedDates)
        {
            InitializeComponent();
            _selectedDates = selectedDates;

            List<DateTime> orderedDates = _selectedDates.OrderBy(d => d.Date).ToList();

            foreach(DateTime date in orderedDates)
            {
                TextBlock text = new TextBlock();
                text.Text = date.ToLongDateString();
                DatesStackPanel.Children.Add(text);
            }
        }

        private void OnClick_Confirm(object sender, RoutedEventArgs e)
        {
            //check if all the dates are valid
            foreach(DateTime d in _selectedDates)
            {
                if (Session.CurrentTemplate is ShiftTemplate)
                {
                    DateTime startTime = new DateTime(d.Year, d.Month, d.Day, Session.CurrentTemplate.StartTime.Hours, Session.CurrentTemplate.StartTime.Minutes, 0);
                    DateTime endTime = startTime.AddHours(Session.CurrentTemplate.Length);
                    try
                    {
                        Session.CurrentRota.CanAddShift(new Shift(startTime, endTime, 52 * (startTime.Year - Session.CurrentRota.RotaStartTime.Year)));
                    }
                    catch (ArgumentException exception)
                    {
                        ErrorMessage.Text= $"Cannot add to rota: {exception.Message}";
                        return;
                    }
                }
                if (Session.CurrentTemplate is OnCallTemplate)
                {
                    object o = Session.CurrentTemplate.GetType().GetProperty("ExpectedHours")?.GetValue(Session.CurrentTemplate, null);
                    double expectedHours = (double)o;

                    DateTime startTime = new DateTime(d.Year, d.Month, d.Day, Session.CurrentTemplate.StartTime.Hours, Session.CurrentTemplate.StartTime.Minutes, 0);
                    DateTime endTime = startTime.AddHours(Session.CurrentTemplate.Length);
                    try
                    {
                        Session.CurrentRota.CanAddOnCall(new OnCallPeriod(startTime, endTime, TimeSpan.FromHours(expectedHours), 52 * (startTime.Year - Session.CurrentRota.RotaStartTime.Year)));
                    }
                    catch (ArgumentException exception)
                    {
                        ErrorMessage.Text = $"Cannot add to rota: {exception.Message}";
                        return;
                    }
                }
            }

            Session.CurrentRota.AddTemplateToDateList(Session.CurrentTemplate, _selectedDates);
            _selectedDates.Clear();
            (Application.Current.MainWindow as MainWindow).ClearGrid();
            (Application.Current.MainWindow as MainWindow).PopulateGrid();
            Close();

        }
        private void OnClick_Cancel(object sender, RoutedEventArgs e)
        {
            Close();

        }
    }
}
