using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RotaChecker;
using RotaChecker.Classes;

namespace RotaChecker.WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Session _session;
        public List<DateTime> SelectedDates = new List<DateTime>();

        public MainWindow()
        {
            InitializeComponent();
            _session = new Session();
            DataContext = _session;
            PopulateGrid();
        }

        internal void PopulateGrid()
        {
            if(_session.CurrentMonth.DaysInMonth.Last().Row == 5)
            {
                RowDefinition row = new RowDefinition();
                row.Name = "ExtraRow";
                row.Height = new GridLength(1, GridUnitType.Star);
                CalendarGrid.RowDefinitions.Add(row);
            }

            for(int i = 0; i < _session.CurrentMonth.DaysInMonth.Count(); i++)
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness(10.0);
                //first row has the date number
                RowDefinition firstRow = new RowDefinition();
                firstRow.Height = GridLength.Auto;
                grid.RowDefinitions.Add(firstRow);

                TextBlock text = new TextBlock();
                text.Text = $"{_session.CurrentMonth.DaysInMonth[i].Date.Day}";

                if (_session.CurrentRota.GetDutiesOnDate(_session.CurrentMonth.DaysInMonth[i].Date) != null)
                {
                    for(int j = 0; j< _session.CurrentRota.GetDutiesOnDate(_session.CurrentMonth.DaysInMonth[i].Date).Count(); j++)
                    {
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(1, GridUnitType.Star);
                        grid.RowDefinitions.Add(row);

                        StackPanel panel = new StackPanel();
                        TextBlock dutyName = new TextBlock();
                        TextBlock dutyTime = new TextBlock();

                        var duty = _session.CurrentRota.GetDutiesOnDate(_session.CurrentMonth.DaysInMonth[i].Date)[j];

                        if (duty is Shift)
                        {
                            if(duty.TemplateName != null)
                            {
                                dutyName.Text = $"Shift: {duty.TemplateName}";
                            }
                            else
                            {
                                dutyName.Text = "Shift";
                            }      
                        }
                        else if(duty is OnCallPeriod)
                        {
                            if (duty.TemplateName != null)
                            {
                                dutyName.Text = $"On Call: {duty.TemplateName}";
                            }
                            else
                            {
                                dutyName.Text = "On Call";
                            }
                        }
                        dutyTime.Text = $"{duty.StartTime.TimeOfDay}";

                        panel.Children.Add(dutyName);
                        panel.Children.Add(dutyTime);

                        Grid.SetRow(panel, j+1);
                        grid.Children.Add(panel);
                    }
                }

                Grid.SetRow(text, 0);
                grid.Children.Add(text);

                Grid.SetRow(grid, _session.CurrentMonth.DaysInMonth[i].Row);
                Grid.SetColumn(grid, _session.CurrentMonth.DaysInMonth[i].Column);

                CalendarGrid.Children.Add(grid);
            }

            GenerateGridLines();

        }
        internal void ClearGrid()
        {
            CalendarGrid.Children.Clear();
            if(CalendarGrid.RowDefinitions.Count > 5)
            {
                CalendarGrid.RowDefinitions.RemoveAt(5);
            }
        }
        private void GenerateGridLines()
        {
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < CalendarGrid.RowDefinitions.Count; j++)
                {
                    Border border = new Border();
                    SolidColorBrush grey = new SolidColorBrush();
                    grey.Color = (Color)ColorConverter.ConvertFromString("#B1BDC7");
                    border.BorderBrush = grey;
                    border.BorderThickness = new Thickness(1.0);
                    border.Background = Brushes.Transparent;
                    border.Margin = new Thickness(1.0);
                    border.CornerRadius = new CornerRadius(3);
                    border.Tag = $"{i},{j}";
                    border.MouseLeftButtonDown += new MouseButtonEventHandler(OnClick_DateSelected);


                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);

                    CalendarGrid.Children.Add(border);
                }
            }
        }

        private void AddToRota_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_session != null)
            {
                if (SelectedDates.Count() > 0 && _session.CurrentTemplate != null)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
        }
        private void AddToRota_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddTemplateToRotaConfirmation confirmationWindow = new AddTemplateToRotaConfirmation(SelectedDates);
            confirmationWindow.Owner = this;
            confirmationWindow.DataContext = _session;
            confirmationWindow.ShowDialog();
        }
        private void RemoveFromRota_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_session != null)
            {
                if(SelectedDates.Count() == 1 && _session.CurrentRota.GetDutiesOnDate(SelectedDates[0].Date).Count() > 0)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
        }
        private void RemoveFromRota_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(_session.CurrentRota.GetDutiesOnDate(SelectedDates[0].Date).Count() > 1)
            {
                SelectShiftToRemove selectShiftToRemove = new SelectShiftToRemove(_session.CurrentRota.GetDutiesOnDate(SelectedDates[0].Date));
                selectShiftToRemove.Owner = this;
                selectShiftToRemove.DataContext = _session;
                selectShiftToRemove.ShowDialog();
            }
            else
            {
                RemoveFromRotaConfirmation removeWindow = new RemoveFromRotaConfirmation(_session.CurrentRota.GetDutiesOnDate(SelectedDates[0].Date)[0]);
                removeWindow.Owner = this;
                removeWindow.DataContext = _session;
                removeWindow.ShowDialog();
            }
        }

        private void OnClick_CreateTemplate(object sender, RoutedEventArgs e)
        {
            AddTemplateWindow addTemplateWindow = new AddTemplateWindow();
            addTemplateWindow.Owner = this;
            addTemplateWindow.DataContext = _session;
            addTemplateWindow.ShowDialog();
        }
        private void OnClick_ChangeMonth(object sender, RoutedEventArgs e)
        {
            int month = _session.CurrentMonth.MonthNumber;
            if (sender.Equals(PreviousMonthButton))
            {
                if(month == 1)
                {
                    _session.CurrentYear -= 1;
                    month = 12;
                }
                else
                {
                    month -= 1;
                }
            }
            if (sender.Equals(NextMonthButton))
            {
                if(month == 12)
                {
                    _session.CurrentYear += 1;
                    month = 1;
                }
                else
                {
                    month += 1;
                }
                
            }
            _session.CurrentMonth = new Month(new DateTime(_session.CurrentYear, month, 1));
            ClearGrid();
            PopulateGrid();

            //check for selected dates
            foreach(GridDateCell date in _session.CurrentMonth.DaysInMonth)
            {
                if(SelectedDates.Contains(date.Date))
                {
                    string coordinate = $"{date.Column},{date.Row}";

                    var rectangles = CalendarGrid.Children.OfType<Rectangle>();

                    foreach(Rectangle rectangle in rectangles)
                    {
                        if(rectangle.Tag.ToString() == coordinate)
                        {
                            SolidColorBrush red = new SolidColorBrush();
                            red.Color = Colors.Red;
                            rectangle.Stroke = red;
                        }
                    }

                }
            }
        }
        private void OnClick_DateSelected(object sender, RoutedEventArgs e)
        {
            Border clickedDate = sender as Border;

            string[] coordinatesArray = clickedDate.Tag.ToString().Split(',');
            int column = Convert.ToInt32(coordinatesArray[0]);
            int row = Convert.ToInt32(coordinatesArray[1]);

            //find the griddatecell with the corresponding coordinate
            GridDateCell selectedGridCell = _session.CurrentMonth.DaysInMonth.FirstOrDefault(i => i.Column == column && i.Row == row);
            if(selectedGridCell != null)
            {
                //Check if it is already selected, and if so then remove it.
                if (SelectedDates.Contains(selectedGridCell.Date))
                {
                    SolidColorBrush grey = new SolidColorBrush();
                    grey.Color = (Color)ColorConverter.ConvertFromString("#B1BDC7");
                    clickedDate.BorderBrush = grey;
                    clickedDate.BorderThickness = new Thickness(1.0);
                    SelectedDates.Remove(selectedGridCell.Date);
                }
                else
                {
                    SolidColorBrush red = new SolidColorBrush();
                    red.Color = (Color)ColorConverter.ConvertFromString("#CC6531");
                    clickedDate.BorderBrush = red;
                    clickedDate.BorderThickness = new Thickness(1.8);
                    SelectedDates.Add(selectedGridCell.Date);
                }

            }
        }
        private void OnClick_OpenCompliance(object sender, RoutedEventArgs e)
        {
            _session.ComplianceChecker = new Compliance(_session.CurrentRota);
            ComplianceWindow complianceWindow = new ComplianceWindow(_session);
            complianceWindow.Owner = this;
            complianceWindow.DataContext = _session;
            complianceWindow.ShowDialog();
        }

    }
}
