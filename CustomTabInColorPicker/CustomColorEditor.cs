using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CustomTabInColorPicker
{
    public class CustomColorEditor : ColorEditor
    {
        public override object EditValue(ITypeDescriptorContext
            context, System.IServiceProvider provider, object value)
        {
            //Get required types and methods
            var ColorUIType = typeof(ColorEditor).GetNestedType("ColorUI",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var ColorUiConstructor = ColorUIType.GetConstructors()[0];
            var ColorEditorListBoxType = ColorUIType.GetNestedType("ColorEditorListBox",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var ColorUiField = typeof(ColorEditor).GetField("colorUI",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var OnListClickMethod = ColorUIType.GetMethod("OnListClick",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var OnListDrawItemMethod = ColorUIType.GetMethod("OnListDrawItem",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var OnListKeyDownMethod = ColorUIType.GetMethod("OnListKeyDown",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Color UI Control
            var colorUi = (Control)ColorUiConstructor.Invoke(new[] { this });
            ColorUiField.SetValue(this, colorUi);

            //Custom colors ListBox
            var listBox = (ListBox)Activator.CreateInstance(ColorEditorListBoxType);
            //Colors
            listBox.Items.AddRange(new object[] { Color.Red, Color.Green, Color.Blue });
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;
            listBox.Sorted = false;
            listBox.Click += (sender, e) =>
                OnListClickMethod.Invoke(colorUi, new[] { sender, e });
            //Custom paint
            listBox.DrawItem += OnListDrawItem;
            //Original paint
            //listBox.DrawItem +=(sender, e) =>
            //    OnListDrawItemMethod.Invoke(colorUi, new[] { sender, e });
            listBox.DrawItem += OnListDrawItem;
            listBox.KeyDown += (sender, e) =>
                OnListKeyDownMethod.Invoke(colorUi, new[] { sender, e });
            listBox.Dock = DockStyle.Fill;

            //Add the custom tab page, including the custome colors
            var tabControl = colorUi.Controls.OfType<TabControl>().First();
            var customTabPage = new TabPage();
            customTabPage.Text = "Theme";
            customTabPage.Controls.Add(listBox);
            tabControl.TabPages.Add(customTabPage);
            return base.EditValue(context, provider, value);
        }
        private void OnListDrawItem(object sender, DrawItemEventArgs e)
        {
            var colorNames = new Dictionary<int, string>
        {
            {Color.Red.ToArgb(), "Blood"},
            {Color.Green.ToArgb(), "Life potion"},
            {Color.Blue.ToArgb(), "Water"},
        };
            ListBox lb = (ListBox)sender;
            object value = lb.Items[e.Index];
            e.DrawBackground();
            this.PaintValue(value, e.Graphics,
                new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, 22, e.Bounds.Height - 4));
            e.Graphics.DrawRectangle(SystemPens.WindowText,
                new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, 22 - 1, e.Bounds.Height - 4 - 1));
            var color = (Color)value;
            var name = colorNames.ContainsKey(color.ToArgb()) ? colorNames[color.ToArgb()] : color.Name;
            using (var foreBrush = new SolidBrush(e.ForeColor))
                e.Graphics.DrawString(name, lb.Font, foreBrush, e.Bounds.X + 26, e.Bounds.Y);
        }
    }
}
