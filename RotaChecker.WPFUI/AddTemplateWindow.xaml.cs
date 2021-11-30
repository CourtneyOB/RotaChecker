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
            if (ShiftButton.IsChecked == true)
            {
                Session.TemplateLibrary.AddTemplate(new ShiftTemplate(TemplateName.Text, 10.0));
            }
            else if(OnCallButton.IsChecked == true)
            {
                Session.TemplateLibrary.AddTemplate(new OnCallTemplate(TemplateName.Text, 10.0, 3.0));
            }
            Close();
        }
    }
}
