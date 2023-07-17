using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomTabInColorPicker
{
    public class MyControl : Control
    {
        [Editor(typeof(CustomColorEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CustomColorConverter))]
        public Color MyColor { get; set; }
    }
}
