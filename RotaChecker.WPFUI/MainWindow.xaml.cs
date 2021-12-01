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

        public MainWindow()
        {
            InitializeComponent();
            _session = new Session();
            DataContext = _session;
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            int offset = 0;

            switch (_session.CurrentMonth.FirstDay)
            {
                case DayOfWeek.Monday:
                    offset = 0;
                    break;
                case DayOfWeek.Tuesday:
                    offset = 1;
                    break;
                case DayOfWeek.Wednesday:
                    offset = 2;
                    break;
                case DayOfWeek.Thursday:
                    offset = 3;
                    break;
                case DayOfWeek.Friday:
                    offset = 4;
                    break;
                case DayOfWeek.Saturday:
                    offset = 5;
                    break;
                case DayOfWeek.Sunday:
                    offset = 6;
                    break;
            }

            for (int i = 0; i < _session.CurrentMonth.DaysInMonth.Count(); i++)
            {
                Grid grid = new Grid();
                RowDefinition firstRow = new RowDefinition();
                firstRow.Height = GridLength.Auto;
                grid.RowDefinitions.Add(firstRow);

                TextBlock text = new TextBlock();
                text.Text = $"{_session.CurrentMonth.DaysInMonth[i].Day}";

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

                if (i + offset < 7)
                {
                    Grid.SetRow(grid, 0);
                    Grid.SetColumn(grid, i + offset);
                }
                else if(i + offset < 14)
                {
                    Grid.SetRow(grid, 1);
                    Grid.SetColumn(grid, i + offset - 7);
                }
                else if(i + offset < 21)
                {
                    Grid.SetRow(grid, 2);
                    Grid.SetColumn(grid, i + offset - 14);
                }
                else if(i + offset < 28)
                {
                    Grid.SetRow(grid, 3);
                    Grid.SetColumn(grid, i + offset - 21);
                }
                else
                {
                    Grid.SetRow(grid, 5);
                    Grid.SetColumn(grid, i + offset - 28);
                }
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
        }
        private void OnClick_DateSelected(object sender, RoutedEventArgs e)
        {
            Rectangle clickedDate = sender as Rectangle;
            TestLabel.Content = $"Date Selected: {clickedDate.Tag}";
        }

    }
}
