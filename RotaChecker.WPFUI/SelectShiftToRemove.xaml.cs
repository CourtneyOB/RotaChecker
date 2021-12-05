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

namespace RotaChecker.WPFUI
{
    /// <summary>
    /// Interaction logic for SelectShiftToRemove.xaml
    /// </summary>
    public partial class SelectShiftToRemove : Window
    {
        private List<WorkDuty> _workDuties;
        private WorkDuty _dutyToRemove;
        public WorkDuty DutyToRemove
        {
            get { return _dutyToRemove; }
            set { _dutyToRemove = value; }
        }
        public List<WorkDuty> WorkDuties
        {
            get { return _workDuties; }
            set { _workDuties = value; }
        }

        public Session Session => DataContext as Session;
        public SelectShiftToRemove(List<WorkDuty> duties)
        {
            InitializeComponent();
            WorkDuties = duties;
        }

        private void OnClick_Confirm(object sender, RoutedEventArgs e)
        {
            Session.CurrentRota.Duties.Remove(DutyToRemove);
            (Application.Current.MainWindow as MainWindow).SelectedDates.Clear();
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
