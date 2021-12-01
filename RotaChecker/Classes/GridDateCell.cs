using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RotaChecker.Classes
{
    public class GridDateCell
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public DateTime Date { get; set; }
        public Rectangle RectangleToggle { get; set; }

        public GridDateCell(DateTime date, int column, int row)
        {
            Date = date;
            Column = column;
            Row = row;

        }

    }
}
