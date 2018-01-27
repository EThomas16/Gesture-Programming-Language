using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Gesture3
{
    //This is the logic behind the Code page
    public partial class CodeWindow : Page
    {
        //Constructor for the code page
        public CodeWindow()
        {
            InitializeComponent();
        }

        //Method to position all elements of the page
        public void init()
        {
            //The colours used in the page
            Brush colourStatement = Brushes.LightGray;
            Brush colourAction = Brushes.Black;
            Brush fontLight = Brushes.White;
            //Calculates the button dimension
            int buttonWidth = Settings.windowWidth / 4;
            int buttonHeight = Settings.windowHeight / 7;
            double fs = Settings.fontScale;

            //Formatting all the UI elements using the staic Formatting class
            Formatting.titleInit(lbl_title, buttonWidth/2, 0, Settings.windowWidth, buttonHeight * 0.8, 42*fs, colourAction, fontLight);
            Formatting.buttonInit(btn_back, buttonWidth / 8, 0, buttonWidth, buttonHeight * 0.8, 42*fs, colourAction, fontLight);

            //Creating statements
            Formatting.buttonInit(btn_if, buttonWidth / 8, (lbl_title.Height * 1.2) + (buttonHeight*0), buttonWidth, buttonHeight, 24*fs, colourAction, colourStatement);
            Formatting.buttonInit(btn_print, buttonWidth / 8, (lbl_title.Height * 1.2) + (buttonHeight * 1), buttonWidth, buttonHeight, 24 * fs, colourAction, colourStatement);
            Formatting.buttonInit(btn_loop, buttonWidth / 8, (lbl_title.Height * 1.2) + (buttonHeight * 2), buttonWidth, buttonHeight, 24 * fs, colourAction, colourStatement);
            Formatting.buttonInit(btn_variable, buttonWidth / 8, (lbl_title.Height * 1.2) + (buttonHeight * 3), buttonWidth, buttonHeight, 24 * fs, colourAction, colourStatement);
            Formatting.buttonInit(btn_exp, buttonWidth / 8, (lbl_title.Height * 1.2) + (buttonHeight * 4), buttonWidth, buttonHeight, 24 * fs, colourAction, colourStatement);

            //Utility
            Formatting.buttonInit(btn_save, Settings.windowWidth - ((buttonWidth/2) * 1.5), Canvas.GetTop(btn_if), buttonWidth/2, buttonHeight, 24*fs, fontLight, colourAction);
            Formatting.buttonInit(btn_run, Settings.windowWidth - ((buttonWidth / 2) * 1.5), Canvas.GetTop(btn_print), buttonWidth / 2, buttonHeight, 24 * fs, fontLight, colourAction);
            Formatting.buttonInit(btn_undo, Settings.windowWidth - ((buttonWidth / 2) * 1.5), Canvas.GetTop(btn_exp), buttonWidth / 2, buttonHeight, 24 * fs, fontLight, colourAction);


            Canvas.SetLeft(codeBorder, Canvas.GetLeft(btn_if) + (btn_if.Width * 1.1));
            Canvas.SetTop(codeBorder, Canvas.GetTop(btn_if));

            codeBorder.Width = buttonWidth * 2;
            codeBorder.Height = buttonHeight * 5;

            //Adds rows to the line numbers shown in the code window
            int count = 0;
            int height = 30;
            if (Settings.windowHeight == 720)
            {
                height = 60;
            }
            foreach (RowDefinition rd in codeWindow.RowDefinitions)
            {
                
                rd.Height = new GridLength(height);
                addLineNum(count);
                count++;
            }

            codeWindow.Width = codeBorder.Width;
            codeWindow.ColumnDefinitions[0].Width = new GridLength(30);
            codeWindow.ColumnDefinitions[1].Width = new GridLength(codeWindow.Width);

            //Calls method to store where the draggable buttons should be positioned
            setDefaultStatementLoc();
        }

        //Method to add a new label to the code window with the given index value
        public void addLineNum(int index)
        {
            //Create a new label which will be the line number
            Label lbl = new Label();
            //Sets the location and colours of the label
            Formatting.labelInit(lbl, 0, 0, 40, 50, 20 * Settings.fontScale, Brushes.Black, Brushes.White, HorizontalAlignment.Left);
            lbl.Content = index + 1;
            //Adds the label to the screen
            codeWindow.Children.Add(lbl);
            //Adds the label to the grid
            Grid.SetColumn(lbl, 0);
            Grid.SetRow(lbl, index);
        }

        //Method to store the current locaions of each draggable button in the Settings class
        public void setDefaultStatementLoc()
        {
            Settings.exp_x = Canvas.GetLeft(btn_exp);
            Settings.exp_y = Canvas.GetTop(btn_exp);
            Settings.if_x = Canvas.GetLeft(btn_if);
            Settings.if_y = Canvas.GetTop(btn_if);
            Settings.loop_x = Canvas.GetLeft(btn_loop);
            Settings.loop_y = Canvas.GetTop(btn_loop);
            Settings.print_x = Canvas.GetLeft(btn_print);
            Settings.print_y = Canvas.GetTop(btn_print);
            Settings.var_x = Canvas.GetLeft(btn_variable);
            Settings.var_y = Canvas.GetTop(btn_variable);
        }

        //Function to get all the interactable buttons in the page
        public List<Button> getInteractableElements()
        {
            List<Button> temp = new List<Button>();
            temp.Add(btn_exp);
            temp.Add(btn_if);
            temp.Add(btn_loop);
            temp.Add(btn_print);
            temp.Add(btn_run);
            temp.Add(btn_save);
            temp.Add(btn_variable);
            temp.Add(btn_undo);
            return temp;
        }
    }
}
