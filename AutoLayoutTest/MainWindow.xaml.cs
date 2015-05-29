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
using AutoLayoutPanel;

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

            var button1 = new Button
            {
                Content = "Foo"
            };
            var button2 = new Button
            {
                Content = "Bar"
            };

            MainPanel.Children.Add(button1);
            MainPanel.Children.Add(button2);

            //button1.Width = 50;
            MainPanel.AddLayoutConstraint(
                button1,
                LayoutProperty.Width,
                "=",
                MainPanel,
                LayoutProperty.Width,
                0.5,
                0);

            // center button 1 in panel (horiz + vert)
            MainPanel.AddLayoutConstraint(
                button1,
                LayoutProperty.HCenter,
                "=",
                MainPanel,
                LayoutProperty.HCenter,
                1,
                0);
            MainPanel.AddLayoutConstraint(
                button1,
                LayoutProperty.VCenter,
                "=",
                MainPanel,
                LayoutProperty.VCenter,
                1,
                0);

            // button2.center = button1.center
            MainPanel.AddLayoutConstraint(
                button2,
                LayoutProperty.Left,
                "=",
                button1,
                LayoutProperty.Left,
                1,
                0);

            // button2.width = 2 * button1.width
            MainPanel.AddLayoutConstraint(
                button2,
                LayoutProperty.Width,
                "=",
                button1,
                LayoutProperty.Width,
                0.5,
                0);

            // button2.top = button1.bottom + 20
            MainPanel.AddLayoutConstraint(
                button2,
                LayoutProperty.Top,
                "=",
                button1,
                LayoutProperty.Bottom,
                1,
                20);
        }
    }
}