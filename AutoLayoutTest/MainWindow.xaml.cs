using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoLayoutTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var panel = new AutoLayoutPanel.AutoLayoutPanel();
            this.Content = panel;

            var button1 = new Button
            {
                Content = "Foo"
            };
            var button2 = new Button
            {
                Content = "Bar"
            };

            panel.Children.Add(button1);
            panel.Children.Add(button2);

            //button1.Width = 50;
            panel.AddLayoutConstraint(button1, "Width", "=", panel, "Width", 0.5, 0);

            // center button 1 in panel (horiz + vert)
            panel.AddLayoutConstraint(button1, "Middle", "=", panel, "Middle", 1, 0);
            panel.AddLayoutConstraint(button1, "Center", "=", panel, "Center", 1, 0);

            // button2.center = button1.center
            panel.AddLayoutConstraint(button2, "Left", "=", button1, "Left", 1, 0);

            // button2.width = 2 * button1.width
            panel.AddLayoutConstraint(button2, "Width", "=", button1, "Width", 0.5, 0);

            // button2.top = button1.bottom + 20
            panel.AddLayoutConstraint(button2, "Top", "=", button1, "Bottom", 1, 20);
        }
    }
}