using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace RotaChecker.WPFUI
{
    public class Commands
    {
        public static readonly RoutedUICommand AddToRota = new RoutedUICommand
            (
            "AddToRota",
            "AddToRota",
            typeof(Commands)
            );
    }
}
