using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RotaChecker;
using RotaChecker.Classes;

namespace RotaChecker.WPFUI
{
    /// <summary>
    /// Interaction logic for AddTemplateWindow.xaml
    /// </summary>
    public partial class AddTemplateWindow : Window
    {
        public Session Session => DataContext as Session;
        public AddTemplateWindow()
        {
            InitializeComponent();
        }

        private void OnClick_SubmitTemplate(object sender, RoutedEventArgs e)
        {
            if (!Validation.ValidateDouble(TemplateLength.Text))
            {
                TemplateLengthError.Text = "Value must be a number";
                return;
            }

            double length = Double.Parse(TemplateLength.Text);
            if(length <= 0)
            {
                TemplateLengthError.Text = "Value must be more than 0";
                return;
            }
            if (length > 24)
            {
                TemplateLengthError.Text = "Value must be less than 24";
                return;
            }

            if (!Validation.ValidateDateTime(StartTime.Text))
            {
                StartTimeError.Text = "Value must be a time in 24 hour format ('HH:mm')";
                return;
            }

            DateTime selectedDate = DateTime.ParseExact(StartTime.Text, "HH:mm", CultureInfo.InvariantCulture);
            TimeSpan startTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, 0);

            if (ShiftButton.IsChecked == true)
            {
                Session.TemplateLibrary.AddTemplate(new ShiftTemplate(TemplateName.Text, startTime, length));
            }
            else if(OnCallButton.IsChecked == true)
            {
                if (!Validation.ValidateDouble(TemplateExpectedHours.Text))
                {
                    TemplateExpectedHoursError.Text = "Value must be a number";
                    return;
                }

                double expectedHours = Double.Parse(TemplateExpectedHours.Text);
                if (expectedHours > length)
                {
                    TemplateExpectedHoursError.Text = "Expected work hours cannot be more than overall length";
                    return;
                }
                if (expectedHours <= 0)
                {
                    TemplateExpectedHoursError.Text = "Value must be more than 0";
                    return;
                }

                Session.TemplateLibrary.AddTemplate(new OnCallTemplate(TemplateName.Text, startTime, length, expectedHours));
            }
            Close();
        }
    }
}
