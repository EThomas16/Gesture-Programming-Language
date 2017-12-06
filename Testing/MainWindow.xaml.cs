/*
 * Authors: Zach Wharton & Erik Thomas
 * Creation Date: 1/12/2017
 * TODO:
 *      Add numbers down the side of the main code box
 *      Check if second form required for variable window
 *      Condense button code down if possible
 */

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

namespace CW2_Main
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

        int boxWidth = 180;
        int boxHeight = 23;
        int fieldHeight;

        public String explorerString = "";
        public String buttonCheck = "";

        Thickness _margin = new Thickness();
        String currentDrag = "";

        List<Button> imports = new List<Button>();
        List<Button> vars = new List<Button>();

        public MainWindow()
        {
            //Maybe condense if possible? Not a necessity
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
            //Handles the moving of the import button to create an import element
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.MouseDevice.Capture(btn_import);
                _margin = btn_import.Margin;
                //Currently selected item, this being the import method import is used
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
            //This can be called when the user releases the push gesture
            e.MouseDevice.Capture(null);

            double importL = importBorder.Margin.Left;
            double importT = importBorder.Margin.Top;
            double importR = importL + importBorder.ActualWidth;
            double importB = importT + importBorder.ActualHeight;

            if (m_MouseY > importT && m_MouseY < importB && m_MouseX > importL && m_MouseX < importR && currentDrag == "import")
            {
                Console.WriteLine("adding imports");
                buttonCheck = "import";
                //Button to hold the currently selected import is created
                Button current_import = new Button();
                importBounds.Children.Add(current_import);
                Grid.SetRow(current_import, importBounds.RowDefinitions.Count);
                //Name of the import is set
                current_import.Name = "import" + imports.Count;
                //Adds the current import to the grid of imports
                //Needs to be spaced out appropriately to prevent overlap of buttons

                //Adds current import to the list of button values
                imports.Add(current_import);
                //Height is set as the height of the button multiplied by the number of imports
                //Could dynamically change width?
                fieldHeight = 34;
                importBounds.Height = fieldHeight * imports.Count;
                Console.WriteLine("importBounds Height: " + importBounds.Height);
                current_import.Height = boxHeight;
                //Can be changed depending on size of text
                current_import.Width = boxWidth;
                current_import.Content = "EMPTY";

                Thickness __margin = new Thickness();
                __margin = current_import.Margin;
                __margin.Left = importL - importR + (current_import.Width);
                Console.WriteLine("Margin left border: " + __margin.Left);
                //Change these values to position the buttons correctly
                if (imports.Count > 1)
                {
                    __margin.Top = imports[imports.Count - 2].Margin.Top - 15;
                }
                else
                {
                    __margin.Top = (importBounds.Height) - importBounds.Height * 1.5;
                }

                current_import.Margin = __margin;

                Console.WriteLine("code window: " + codeWindow.Margin.Top);
                Console.WriteLine("new button: " + current_import.Margin.Top);
                //EDITED
                buttonCheck = "import";

                Form1 importExplorer = new Form1(this, current_import, buttonCheck);
                importExplorer.Show();

                //mainGrd.Children.Remove(btn_import);
                //importGrid.Children.Add(btn_import);

                foreach (FrameworkElement element in importBounds.Children)
                {
                    //try { }
                    Console.WriteLine(element);
                }
            }
            //EDITED to fit with main code window
            Thickness codeWindowMargin = new Thickness();
            codeWindowMargin = codeBorder.Margin;

            Thickness varMargin = new Thickness();
            varMargin = variableBorder.Margin;

            importBorder.Height += boxHeight;
            importBounds.Height += boxHeight;
            codeWindowMargin.Top += boxHeight;
            varMargin.Top += boxHeight;

            codeBorder.Margin = codeWindowMargin;
            variableBorder.Margin = varMargin;

            importBounds.RowDefinitions.Add(new RowDefinition());

            Thickness _margin = new Thickness();
            _margin = btn_import.Margin;
            _margin.Left = starting_x;
            _margin.Top = starting_y;
            btn_import.Margin = _margin;
        }

        private void var_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(null);
            //EDITED
            double variableL = variableBorder.Margin.Left;
            double variableT = variableBorder.Margin.Top;
            double variableR = variableL + variableBorder.ActualWidth;
            double variableB = variableT + variableBorder.ActualHeight;

            if (m_MouseY > variableT && m_MouseY < variableB && m_MouseX > variableL && m_MouseX < variableR && currentDrag == "variable")
            {
                Console.WriteLine("Open variable creator");
                buttonCheck = "var";
                Button current_var = new Button();
                variableBounds.Children.Add(current_var);
                Grid.SetRow(current_var, variableBounds.RowDefinitions.Count);

                current_var.Name = "variable" + vars.Count;
                Console.WriteLine("Current variable name: " + current_var.Name);
                vars.Add(current_var);
                //Sets values for variable storage box
                fieldHeight = 34;
                variableBounds.Height = fieldHeight * vars.Count - 30;
                current_var.Height = boxHeight;
                current_var.Width = boxWidth;
                current_var.Content = "EMPTY";

                Thickness __margin = new Thickness();
                __margin = current_var.Margin;
                __margin.Left = variableL - variableR + (current_var.Width);
                //Change these depending on positioning of buttons
                if (vars.Count > 1)
                {
                    __margin.Top = vars[vars.Count - 2].Margin.Top - 30;
                }
                else
                {
                    __margin.Top = (variableBounds.Height) - variableBounds.Height * 1.5;
                }
                //EDITED
                buttonCheck = "var";
                Form1 varExplorer = new Form1(this, current_var, buttonCheck);
                varExplorer.Show();

                foreach (FrameworkElement element in variableBounds.Children)
                {
                    Console.WriteLine(element);
                }
            }
            //EDITED to fit with main code window
            Thickness codeWindowMargin = new Thickness();
            codeWindowMargin = codeBorder.Margin;

            variableBorder.Height += boxHeight;
            variableBounds.Height += boxHeight;
            codeWindowMargin.Top += boxHeight;

            codeBorder.Margin = codeWindowMargin;

            variableBounds.RowDefinitions.Add(new RowDefinition());

            Thickness _margin = new Thickness();
            _margin = btn_import.Margin;
            _margin.Left = starting_x;
            _margin.Top = starting_y;
            btn_import.Margin = _margin;

        }
    }

}
