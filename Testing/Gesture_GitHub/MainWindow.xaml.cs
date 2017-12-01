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

namespace GestureGUITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Control activeCont;
        private Point prevLoc;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonOne_Click(object sender, RoutedEventArgs e)
        {
            var box = new TextBox();
            box.Location = new Point(50, 50);
            box.MouseDown += new MouseEventHandler(textboxMouseDown);
        }

        private void textboxMouseDown(object sender, MouseButtonEventArgs e)
        {
            activeCont = sender as Control;
            prevLoc = e.Location;
            Cursor = Cursors.Hand;
        }

        private void textboxMouseMove(object sender, MouseEventArgs e)
        {
            if (activeCont == null || activeCont != sender)
            {
                return;
            }
            var location = activeCont.Location;
            location.Offset(e.Location.X - prevLoc.X, e.Location.Y - prevLoc.Y);
            activeCont.Location = location;
        }
    }
}
