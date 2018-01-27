using System;
using System.IO;
using System.Windows;
using System.Speech.Synthesis;

namespace Gesture3
{
    //This is the logic behind the MainWindow page (The starting class for the program)
    public partial class MainWindow : Window
    {
        //An instance of the kinect class is created where the majority of the logic takes place
        Kinect mainLogic;

        //Constructor for this class
        public MainWindow()
        {
            InitializeComponent();
            //Method call to initialise the UI elements
            init();
        }

        //Method to position UI elements of the page relative to the screen size
        public void init()
        {
            //Initialise the default program settings on first load
            Settings.fontScale = 1.0;
            Settings.rightHandTracked = true;
            Settings.windowWidth = 640;
            Settings.windowHeight = 480;
            Settings.ttsVolume = 100;
            String dir = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "\\resources\\";
            Settings.resourcePath = dir;

            //Initialise the Kinect, passing an instance of this class to allow the program to return
            mainLogic = new Kinect(this);
            //Start up the Kinect features
            mainLogic.initiate_kinect();
            //Tell the kinect to set this page as the content of the window
            mainLogic.openMenu();
            //Audibly tell the user that the program is ready
            textToSpeech("program loaded");
        }

        //Method to set the size of this page as specified in the Settings class
        public void scaleSize()
        {
            this.Width = Settings.windowWidth;
            this.Height = Settings.windowHeight;
        }

        //Method that enables the program to provide audible feedback to the user
        public void textToSpeech(String input)
        {
            //Instance the Speech Synthesizer class
            SpeechSynthesizer reader = new SpeechSynthesizer();
            //Apply the currently selected volume option
            reader.Volume = Settings.ttsVolume;
            //Speak while not interupting the main thread
            reader.SpeakAsync(input);
        }
    }
}
