using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QGameAssignment_3
{
    public partial class Tile : PictureBox
    {
        public string tileColor { get; set; }
        public string tileType { get; set; }
        public bool selected { get; set; }
    }
}
