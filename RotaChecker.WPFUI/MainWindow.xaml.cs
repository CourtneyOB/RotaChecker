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
        private readonly Session _session;
        private List<DateTime> _selectedDates = new List<DateTime>();

        public MainWindow()
        {
            InitializeComponent();
            _session = new Session();
            DataContext = _session;
            PopulateGrid();
        }

        internal void PopulateGrid()
        {

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
        }
        private void GenerateGridLines()
        {
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    SolidColorBrush grey = new SolidColorBrush();
                    grey.Color = Colors.LightGray;
                    SolidColorBrush transparent = new SolidColorBrush();
                    transparent.Color = Colors.Transparent;
                    rectangle.Stroke = grey;
                    rectangle.Fill = transparent;
                    rectangle.Margin = new Thickness(1.0);
                    rectangle.Tag = $"{i},{j}";
                    rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(OnClick_DateSelected);


                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);

                    CalendarGrid.Children.Add(rectangle);
                }
            }
        }

        private void AddToRota_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_session != null)
            {
                if (_selectedDates != null && _session.CurrentTemplate != null)
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
            AddTemplateToRotaConfirmation confirmationWindow = new AddTemplateToRotaConfirmation(_selectedDates);
            confirmationWindow.Owner = this;
            confirmationWindow.DataContext = _session;
            confirmationWindow.ShowDialog();
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
                if(_selectedDates.Contains(date.Date))
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
            Rectangle clickedDate = sender as Rectangle;

            string[] coordinatesArray = clickedDate.Tag.ToString().Split(',');
            int column = Convert.ToInt32(coordinatesArray[0]);
            int row = Convert.ToInt32(coordinatesArray[1]);

            //find the griddatecell with the corresponding coordinate
            GridDateCell selectedGridCell = _session.CurrentMonth.DaysInMonth.FirstOrDefault(i => i.Column == column && i.Row == row);
            if(selectedGridCell != null)
            {
                //Check if it is already selected, and if so then remove it.
                if (_selectedDates.Contains(selectedGridCell.Date))
                {
                    SolidColorBrush grey = new SolidColorBrush();
                    grey.Color = Colors.LightGray;
                    clickedDate.Stroke = grey;
                    _selectedDates.Remove(selectedGridCell.Date);
                }
                else
                {
                    SolidColorBrush red = new SolidColorBrush();
                    red.Color = Colors.Red;
                    clickedDate.Stroke = red;
                    _selectedDates.Add(selectedGridCell.Date);
                }

            }
        }

    }
}
