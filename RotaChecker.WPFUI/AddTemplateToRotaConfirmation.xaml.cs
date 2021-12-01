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
    public partial class AddTemplateToRotaConfirmation : Window
    {
        private readonly List<DateTime> _selectedDates = new List<DateTime>();
        public Session Session => DataContext as Session;
        public AddTemplateToRotaConfirmation(List<DateTime> selectedDates)
        {
            InitializeComponent();
            _selectedDates = selectedDates;

            foreach(DateTime date in _selectedDates)
            {
                TextBlock text = new TextBlock();
                text.Text = date.ToString();
                DatesStackPanel.Children.Add(text);
            }
        }

        private void OnClick_Confirm(object sender, RoutedEventArgs e)
        {
            //check if all the dates are valid
            foreach(DateTime date in _selectedDates)
            {

            }
        }
    }
}
