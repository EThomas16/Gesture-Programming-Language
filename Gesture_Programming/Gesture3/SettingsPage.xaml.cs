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

namespace Gesture3
{
    //This is the logic behind the settings page
    public partial class SettingsPage : Page
    {
        //Basic Constructor to load the page
        public SettingsPage()
        {
            InitializeComponent();
        }

        //This method initialises the location of each UI element for this page (Called from the kinect class)
        public void init()
        {
            //Gets the current window size from the settings class
            this.Width = Settings.windowWidth;
            this.Height = Settings.windowHeight;

            //Gets the current font scale from the settings class
            double fs = Settings.fontScale;
            //Calculate the dimensions for the button of this page relative to the screen dimenions
            int buttonHeight = Settings.windowHeight / 8;
            int buttonWidth = Settings.windowWidth / 4;
            
            //Uses the formating class to put each element into the correct location based on the screen size
            Formatting.titleInit(lbl_title, buttonWidth / 1.5, 0, Settings.windowWidth, buttonHeight * 1.2, 48 * fs, Brushes.Gold, Brushes.Purple, HorizontalAlignment.Left);
            Formatting.buttonInit(btn_back, 0, 0, buttonWidth / 1.5, buttonHeight * 1.2, 28*fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_apply, (Settings.windowWidth / 2) - (buttonWidth / 2), Settings.windowHeight - (buttonHeight * 2), buttonWidth, buttonHeight, 36*fs, Brushes.Gold, Brushes.Purple);

            Formatting.labelInit(lbl_hand, 0, lbl_title.Height + (buttonHeight * 1), buttonWidth * 1.2, buttonHeight, 20 * fs, Brushes.White, Brushes.Black, HorizontalAlignment.Right);
            Formatting.labelInit(lbl_screenSize, 0, lbl_title.Height + (buttonHeight * 2), buttonWidth * 1.2, buttonHeight, 20 * fs, Brushes.White, Brushes.Black, HorizontalAlignment.Right);
            Formatting.labelInit(lbl_textSize, 0, lbl_title.Height + (buttonHeight * 3), buttonWidth * 1.2, buttonHeight, 20 * fs, Brushes.White, Brushes.Black, HorizontalAlignment.Right);
            Formatting.labelInit(lbl_Volume, 0, lbl_title.Height + (buttonHeight * 4), buttonWidth * 1.2, buttonHeight, 20 * fs, Brushes.White, Brushes.Black, HorizontalAlignment.Right);

            Formatting.buttonInit(btn_Left, Canvas.GetLeft(lbl_hand) + lbl_hand.Width * 1.2, Canvas.GetTop(lbl_hand), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_Right, Canvas.GetLeft(lbl_hand) + lbl_hand.Width * 1.2 + btn_Left.Width, Canvas.GetTop(lbl_hand), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_normScreen, Canvas.GetLeft(lbl_screenSize) + lbl_screenSize.Width * 1.2, Canvas.GetTop(lbl_screenSize), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_largeScreen, Canvas.GetLeft(lbl_screenSize) + lbl_screenSize.Width * 1.2 + btn_normScreen.Width, Canvas.GetTop(lbl_screenSize), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_normText, Canvas.GetLeft(lbl_textSize) + lbl_textSize.Width * 1.2, Canvas.GetTop(lbl_textSize), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_largeText, Canvas.GetLeft(lbl_textSize) + lbl_textSize.Width * 1.2 + btn_normText.Width, Canvas.GetTop(lbl_textSize), buttonWidth, buttonHeight, 36 * fs, Brushes.White, Brushes.Black);

            //Sets the colour of the applied settings options based on the current Settings values
            setCurrentSettings();
        }

        //Method to visually display which options are selected
        public void setCurrentSettings()
        {
            //A series of If statements to change the correct button colour
            if (Settings.rightHandTracked)
            {
                btn_Right.Background = Brushes.DarkGreen;
            }
            else
            {
                btn_Left.Background = Brushes.DarkGreen;
            }

            if (Settings.fontScale == 1.0)
            {
                btn_normText.Background = Brushes.DarkGreen;
            }
            else
            {
                btn_largeText.Background = Brushes.DarkGreen;
            }

            if (Settings.windowHeight == 480)
            {
                btn_normScreen.Background = Brushes.DarkGreen;
            }
            else
            {
                btn_largeScreen.Background = Brushes.DarkGreen;
            }
        }

        //Function to return a list of all interactable button elements of this page
        public List<Button> getInteractableElements()
        {
            List<Button> temp = new List<Button>();
            temp.Add(btn_back);
            temp.Add(btn_apply);
            temp.Add(btn_Left);
            temp.Add(btn_Right);
            temp.Add(btn_normScreen);
            temp.Add(btn_largeScreen);
            temp.Add(btn_normText);
            temp.Add(btn_largeText);
            return temp;
        }
    }
}
