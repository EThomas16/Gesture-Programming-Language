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

namespace DragDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double m_MouseX;
        double m_MouseY;

        double starting_x;
        double starting_y;

        public String explorerString = "";

        Thickness _margin = new Thickness();
        String currentDrag = "";
        public MainWindow()
        {
            InitializeComponent();
            btn_import.PreviewMouseUp += new MouseButtonEventHandler(imp_MouseUp);
            btn_import.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(imp_MouseLeftButtonUp);
            btn_import.PreviewMouseMove += new MouseEventHandler(btn_import_MouseMove);

            btn_variable.PreviewMouseUp += new MouseButtonEventHandler(var_MouseUp);
            btn_variable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(var_MouseLeftButtonUp);
            btn_variable.PreviewMouseMove += new MouseEventHandler(btn_variable_MouseMove);

            starting_x = btn_import.Margin.Left;
            starting_y = btn_import.Margin.Top;
        }



        private void imp_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the Position of Window so that it will set margin from this window

            m_MouseX = e.GetPosition(this).X;
            m_MouseY = e.GetPosition(this).Y;


        }

        private void var_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the Position of Window so that it will set margin from this window

            m_MouseX = e.GetPosition(this).X;
            m_MouseY = e.GetPosition(this).Y;


        }

        private void btn_import_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.MouseDevice.Capture(btn_import);
                _margin = btn_import.Margin;
                currentDrag = "import";
                // Capture the mouse for border
                int _tempX = Convert.ToInt32(e.GetPosition(this).X);
                int _tempY = Convert.ToInt32(e.GetPosition(this).Y);
                // when While moving _tempX get greater than m_MouseX relative to usercontrol 
                if (m_MouseX > _tempX)
                {
                    // add the difference of both to Left
                    _margin.Left += (_tempX - m_MouseX);
                    // subtract the difference of both to Left
                    _margin.Right -= (_tempX - m_MouseX);
                }
                else
                {
                    _margin.Left -= (m_MouseX - _tempX);
                    _margin.Right -= (_tempX - m_MouseX);
                }
                if (m_MouseY > _tempY)
                {
                    _margin.Top += (_tempY - m_MouseY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                else
                {
                    _margin.Top -= (m_MouseY - _tempY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                btn_import.Margin = _margin;
                m_MouseX = _tempX;
                m_MouseY = _tempY;
            }
        }

        private void btn_variable_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.MouseDevice.Capture(btn_variable);
                _margin = btn_variable.Margin;
                currentDrag = "variable";
                // Capture the mouse for border
                int _tempX = Convert.ToInt32(e.GetPosition(this).X);
                int _tempY = Convert.ToInt32(e.GetPosition(this).Y);
                // when While moving _tempX get greater than m_MouseX relative to usercontrol 
                if (m_MouseX > _tempX)
                {
                    // add the difference of both to Left
                    _margin.Left += (_tempX - m_MouseX);
                    // subtract the difference of both to Left
                    _margin.Right -= (_tempX - m_MouseX);
                }
                else
                {
                    _margin.Left -= (m_MouseX - _tempX);
                    _margin.Right -= (_tempX - m_MouseX);
                }
                if (m_MouseY > _tempY)
                {
                    _margin.Top += (_tempY - m_MouseY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                else
                {
                    _margin.Top -= (m_MouseY - _tempY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                btn_variable.Margin = _margin;
                m_MouseX = _tempX;
                m_MouseY = _tempY;
            }
        }

        private void imp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(null);

            double importL = importBounds.Margin.Left;
            double importT = importBounds.Margin.Top;
            double importR = importL + importBounds.ActualWidth;
            double importB = importT + importBounds.ActualHeight;

            if (m_MouseY > importT && m_MouseY < importB && m_MouseX > importL && m_MouseX < importR && currentDrag == "import")
            {
                Console.WriteLine("adding imports");
                Form1 importExplorer = new Form1(this);
                importExplorer.Show();
                Console.WriteLine(explorerString);
                explorerString = "";
            }
            else
            {
                Thickness _margin = new Thickness();
                _margin = btn_import.Margin;
                _margin.Left = starting_x;
                _margin.Top = starting_y;
                btn_import.Margin = _margin;
            }
        }

        private void var_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(null);

            double variableL = variableBounds.Margin.Left;
            double variableT = variableBounds.Margin.Top;
            double variableR = variableL + variableBounds.ActualWidth;
            double variableB = variableT + variableBounds.ActualHeight;
            
            if (m_MouseY > variableT && m_MouseY < variableB && m_MouseX > variableL && m_MouseX < variableR && currentDrag == "variable")
            {
                Console.WriteLine("Open variable creator");
            }
            else
            {
                Thickness _margin = new Thickness();
                _margin = btn_import.Margin;
                _margin.Left = starting_x;
                _margin.Top = starting_y;
                btn_import.Margin = _margin;
            }
        }
    }

}