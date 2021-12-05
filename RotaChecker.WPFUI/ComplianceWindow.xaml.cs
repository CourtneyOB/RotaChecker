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
    public partial class ComplianceWindow : Window
    {
        private readonly Session _session;
        public ComplianceWindow(Session session)
        {
            InitializeComponent();
            _session = session;
            DataContext = _session;
            this.Closed += new EventHandler(ComplianceWindow_Closed);
            Populate();
        }

        private void Populate()
        {
            string text;

            if (_session.ComplianceChecker.Max48PerWeek(out text))
            {
                Max48PerWeekImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max48PerWeekImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max48PerWeek.Text = text;

            if (_session.ComplianceChecker.Max72Per168(out text))
            {
                Max72Per168Img.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max72Per168Img.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max72Per168.Text = text;

            if (_session.ComplianceChecker.Max13HourShift(out text))
            {
                Max13HourShiftImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max13HourShiftImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max13HourShift.Text = text;

            if (_session.ComplianceChecker.Max4LongShifts(out text))
            {
                Max4LongShiftsImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max4LongShiftsImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max4LongShifts.Text = text;

            if (_session.ComplianceChecker.Max7ConsecutiveDays(out text))
            {
                Max7ConsecutiveDaysImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max7ConsecutiveDaysImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max7ConsecutiveDays.Text = text;

            if (_session.ComplianceChecker.AtLeast11HoursRest(out text))
            {
                AtLeast11HoursRestImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                AtLeast11HoursRestImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            AtLeast11HoursRest.Text = text;

            if (_session.ComplianceChecker.NightRestBreaks(out text))
            {
                NightRestBreaksImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                NightRestBreaksImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            NightRestBreaks.Text = text;

            if (_session.ComplianceChecker.WeekendFrequency(out text))
            {
                WeekendFrequencyImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                WeekendFrequencyImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            WeekendFrequency.Text = text;

            if (_session.ComplianceChecker.Max24HourOnCall(out text))
            {
                Max24HourOnCallImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                Max24HourOnCallImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            Max24HourOnCall.Text = text;

            if (_session.ComplianceChecker.NoConsecutiveOnCallPeriods(out text))
            {
                NoConsecutiveOnCallPeriodsImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                NoConsecutiveOnCallPeriodsImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            NoConsecutiveOnCallPeriods.Text = text;

            if (_session.ComplianceChecker.NoMoreThan3OnCallsIn7Days(out text))
            {
                NoMoreThan3OnCallsIn7DaysImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                NoMoreThan3OnCallsIn7DaysImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            NoMoreThan3OnCallsIn7Days.Text = text;

            if (_session.ComplianceChecker.DayAfterOnCallMustNotHaveWorkLongerThan10Hours(out text))
            {
                DayAfterOnCallMustNotHaveWorkLongerThan10HoursImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                DayAfterOnCallMustNotHaveWorkLongerThan10HoursImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            DayAfterOnCallMustNotHaveWorkLongerThan10Hours.Text = text;

            if (_session.ComplianceChecker.EightHoursRestPer24HourOnCall(out text))
            {
                EightHoursRestPer24HourOnCallImg.Source = new BitmapImage(new Uri("/Img/pass.png", UriKind.Relative));
            }
            else
            {
                EightHoursRestPer24HourOnCallImg.Source = new BitmapImage(new Uri("/Img/fail.png", UriKind.Relative));
            }
            EightHoursRestPer24HourOnCall.Text = text;
        }

        private void ComplianceWindow_Closed(object sender, EventArgs e)
        {
            this.Owner.Show();
        }
    }
}
