using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gesture3
{
    //This is the logic behind the help page
    public partial class HelpPage : Page
    {

        //Stores the information of the tutorial gifs
        struct tutorials
        {
            public String title;
            public Uri url;
        }

        tutorials[] tuts;
        int currentGif = 0;

        //Constructor
        public HelpPage()
        {
            InitializeComponent();
        }

        //Method to position each of the UI elements for this page
        public void init()
        {
            double fs = Settings.fontScale;
            int buttonWidth = Settings.windowWidth / 3;
            int buttonHeight = Settings.windowHeight / 8;

            Formatting.titleInit(lbl_title, buttonWidth/2, 0, Settings.windowWidth, buttonHeight, 48*fs, Brushes.Gold, Brushes.Purple, HorizontalAlignment.Left);

            Formatting.labelInit(lbl_content, 0, lbl_title.Height + (buttonHeight * 1.2), Settings.windowWidth, Settings.windowHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.labelInit(lbl_gifHeading, 0, lbl_title.Height + (buttonHeight * 1.2), Settings.windowWidth, lbl_gifHeading.Height, 28 * fs, Brushes.White, Brushes.Black);

            Formatting.buttonInit(btn_back, 0, 0, buttonWidth/2, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_using, buttonWidth * 0, lbl_title.Height, buttonWidth, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_gestures, buttonWidth * 1, lbl_title.Height, buttonWidth, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_commands, buttonWidth * 2, lbl_title.Height, buttonWidth, buttonHeight, 20 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_goLeft, (buttonWidth / 2.3), lbl_title.Height * 3, buttonWidth / 3, Settings.windowHeight - lbl_title.Height - (buttonHeight*2.75), 34 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_goRight, Settings.windowWidth - (buttonWidth / 1.3), lbl_title.Height * 3, buttonWidth / 3, Settings.windowHeight - lbl_title.Height - (buttonHeight * 2.75), 34 * fs, Brushes.White, Brushes.Black);            

            gestureGrid.Width = Settings.windowWidth / 2;
            gestureGrid.Height = Settings.windowHeight;
            Canvas.SetLeft(gestureGrid, (Settings.windowWidth / 2) - (gestureGrid.Width / 2));
            Canvas.SetTop(gestureGrid, Canvas.GetTop(btn_using) + (btn_gestures.Height * 2));
            gestureGrid.HorizontalAlignment = HorizontalAlignment.Center;

            commandGrid.Width = Settings.windowWidth / 1.5;
            commandGrid.Height = Settings.windowHeight;
            
            Canvas.SetLeft(commandGrid, (Settings.windowWidth / 2) - (commandGrid.Width / 2));
            Canvas.SetTop(commandGrid, Canvas.GetTop(btn_using) + (btn_using.Height * 1.3));

            commandGrid.HorizontalAlignment = HorizontalAlignment.Center;
            //Sets the height of the rows for the command grid
            foreach (RowDefinition rd in commandGrid.RowDefinitions)
            {
                rd.MaxHeight = buttonHeight / 3;
            }
            //Sets the height of the rows for the gesture grid
            foreach (RowDefinition rd in gestureGrid.RowDefinitions)
            {
                rd.MaxHeight = buttonHeight / 2;
            }

            initTutorials();
        }

        //Sets the URL of the tutorial gifs
        public void initTutorials()
        {

            tuts = new tutorials[4];

            //Set the size and position of the gif
            tutorialGif.Width = Settings.windowWidth / 2;
            tutorialGif.Height = btn_goLeft.Height;
            Canvas.SetLeft(tutorialGif, Canvas.GetLeft(btn_goLeft) + btn_goLeft.Width);
            Canvas.SetTop(tutorialGif, Canvas.GetTop(btn_goLeft));

            //Set the file path for each of the gifs
            for (int i = 0; i < 4; i++)
            {
                tuts[i].url = new Uri(Settings.resourcePath + "gif" + i + ".gif", UriKind.RelativeOrAbsolute);
            }

            //Set the title for each gif
            tuts[0].title = "Hand Interactions";
            tuts[1].title = "Gestures";
            tuts[2].title = "Adding Code";
            tuts[3].title = "Running Programs";

            //Setup the first gif that will be seen on load
            tutorialGif.Source = tuts[0].url;
            lbl_gifHeading.Content = tuts[0].title;
            
        }

        //Goes back 1 gif in the list
        public void previousGif()
        {
            //Check that the user is not at the first gif
            if (currentGif > 0)
            {
                //decrement the current place
                currentGif--;
                //change the title and gif to the previous one
                tutorialGif.Source = tuts[currentGif].url;
                lbl_gifHeading.Content = tuts[currentGif].title;
                btn_goRight.IsEnabled = true;
            }
            //Disable the button to prevent the user going back
            if (currentGif == 0)
            {
                btn_goLeft.IsEnabled = false;
            }
        }

        //Goes forward 1 gif in the list
        public void nextGif()
        {
            //Check that the user is not at the last gif in the array
            if (currentGif < 3)
            {
                //Increment the current place
                currentGif++;
                //Set the title and gif
                tutorialGif.Source = tuts[currentGif].url;
                lbl_gifHeading.Content = tuts[currentGif].title;
                //Allow the user to go back a gif
                btn_goLeft.IsEnabled = true;
            }
            //If the end of the array is reached, stop this button from being pressed
            if (currentGif == 3)
            {
                btn_goRight.IsEnabled = false;
            }
        }

        //Function to get all the interactable buttons in the page
        public List<Button> getInteractableElements()
        {
            List<Button> temp = new List<Button>();
            temp.Add(btn_back);
            temp.Add(btn_using);
            temp.Add(btn_gestures);
            temp.Add(btn_commands);
            temp.Add(btn_goLeft);
            temp.Add(btn_goRight);
            return temp;
        }

        //Method to make the gifs loop on completion
        private void tutorialGif_MediaEnded(object sender, RoutedEventArgs e)
        {
            tutorialGif.Position = TimeSpan.FromMilliseconds(1);
        }
    }
}
