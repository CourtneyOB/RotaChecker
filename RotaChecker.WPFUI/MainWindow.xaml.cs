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

        private void PopulateGrid()
        {

            for(int i = 0; i < _session.CurrentMonth.DaysInMonth.Count(); i++)
            {
                Grid grid = new Grid();
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

                        TextBlock eventText = new TextBlock();
                        eventText.Text = $"{_session.CurrentRota.GetDutiesOnDate(_session.CurrentMonth.DaysInMonth[i].Date)[j].StartTime}";
                        Grid.SetRow(eventText, j+1);
                        grid.Children.Add(eventText);
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
        private void ClearGrid()
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
                    rectangle.Tag = $"{i},{j}";
                    rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(OnClick_DateSelected);


                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);

                    CalendarGrid.Children.Add(rectangle);
                }
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
