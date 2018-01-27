using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gesture3
{
    //This is the logic behind the menu page
    public partial class Menu : Page
    {
        //Basic Constructor for the Menu class
        public Menu()
        {
            InitializeComponent();
        }

        //Method to position each of the elements relative to the screen size
        public void init()
        {
            //The colours used for the buttons
            Brush buttonBackground = Brushes.Black;
            Brush buttonFontColour = Brushes.White;
            //Calculate the size of the buttons
            int buttonWidth = Convert.ToInt32(Settings.windowWidth / 3);
            int buttonHeight = Convert.ToInt32(Settings.windowHeight / 8);
            //Calculate the size of the button fonts with the scaling applied
            double buttonFontSize = 36 * Settings.fontScale;

            //Sets the background colour of the menu
            this.Background = Brushes.DarkGreen;

            //Formats the title label using the static class
            Formatting.titleInit(lbl_title, 0, 0, Settings.windowWidth, buttonHeight * 1.3, 48 * Settings.fontScale, Brushes.Gold, Brushes.Purple);

            //Uses a loop to go through and apply the size and formatting of each button on the page
            Button[] btn = {btn_new, btn_open, btn_help, btn_settings, btn_quit};
            for (int i = 1; i < 6; i++)
            {
                int x = buttonWidth;
                int y = Convert.ToInt32(lbl_title.Height / 2) + (buttonHeight * i);
                Formatting.buttonInit(btn[i-1], x, y, buttonWidth, buttonHeight, buttonFontSize, buttonFontColour, buttonBackground);
            }
        }

        //Function to return all of the interactable buttons on this page
        public List<Button> getInteractableElements()
        {
            List<Button> temp = new List<Button>();
            temp.Add(btn_help);
            temp.Add(btn_new);
            temp.Add(btn_open);
            temp.Add(btn_quit);
            temp.Add(btn_settings);

            return temp;
        }
    }
}
