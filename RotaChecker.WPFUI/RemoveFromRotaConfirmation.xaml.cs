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
    /// Interaction logic for RemoveFromRotaConfirmation.xaml
    /// </summary>
    public partial class RemoveFromRotaConfirmation : Window
    {
        private WorkDuty _dutyToRemove;
        public Session Session => DataContext as Session;
        public RemoveFromRotaConfirmation(WorkDuty duty)
        {
            InitializeComponent();
            _dutyToRemove = duty;
            DutyToRemove.Text = duty.StartTime.ToString();
        }

        private void OnClick_Confirm(object sender, RoutedEventArgs e)
        {
            Session.CurrentRota.Duties.Remove(_dutyToRemove);
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
