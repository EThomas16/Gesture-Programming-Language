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
using System.Windows.Shapes;

namespace Test_Wpf {
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window {

        MainWindow back;
        public TestWindow(MainWindow back) {
            InitializeComponent();
            this.back = back;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Visibility = Visibility.Hidden;
            back.Visibility = Visibility.Visible;
        }
    }
}
