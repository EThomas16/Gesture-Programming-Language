using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gesture3
{
    //This class handles the programs use of Speech to text and voice commands
    class Speech
    {
        //Speech recognition engine for the commands list
        private SpeechRecognitionEngine sre;
        //Speech recognition engine for speech to text
        private System.Speech.Recognition.SpeechRecognitionEngine stt_sre;
        //Audio thread for commands list
        private Thread audioThread;
        //Stores the current result from speech detection
        private String speechResult = "";
        //Used as a check for setting variable names and content
        public String varResult = "";
        //Stores the currently active button
        public Button currentBtn;
        //Instances of other classes
        Kinect kinect;
        Event events;
        Gesture gst;

        public Speech(Kinect kinect)
        {
            //Constructor for speech, uses methods from kinect, events and gestures, with the latter two being called through kinect
            this.kinect = kinect;
            events = kinect.gst.events;
            gst = kinect.gst;
        }

        public void speechInit()
        {
            //Initialises the speech recognition engine using the commands list
            //Calls the kinect recogniser method and sets the value returned as a speec recogniser
            RecognizerInfo ri = kinectRecogniser();
            //Creates an instance of the speech recognition engine
            sre = new SpeechRecognitionEngine(ri.Id);
            //Sets the commands available for speech recognition
            var commands = getChoices();
            //Creates an instance of a grammar builder for use with the commands speech recongition engine
            var builder = new GrammarBuilder();
            //Sets the language of the grammar builder
            builder.Culture = ri.Culture;
            //Appends the commands list to the grammar builder
            builder.Append(commands);
            //Uses the grammar builder for grammar recognition
            var grammar = new Grammar(builder);
            sre.LoadGrammar(grammar);
            //Creates an event for when speech is recognised, calling the speecRecognised method when speech is detected
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speechRecognised);
            //Creates a new audio thread to listen for audio input
            audioThread = new Thread(audioListen);
            audioThread.Start();
        }

        public void speechToTextInit()
        {
            //This method is used for converting speech into text
            //Creates an instance of the system.speech speech recognition engine for use with speech to text
            stt_sre = new System.Speech.Recognition.SpeechRecognitionEngine();
            //Uses dictation grammar to allow freedom of speech for entering any word detected
            System.Speech.Recognition.Grammar dictationGrammar = new System.Speech.Recognition.DictationGrammar();
            //Loads the dictation grammar into the speech recognition engine
            stt_sre.LoadGrammar(dictationGrammar);

            try
            {
                //Try catch is used here to catch any invalid operation exceptions that could occur when detecting speech to text
                stt_sre.SetInputToDefaultAudioDevice();
                //Saves result from speech recognition as a recognition result object
                System.Speech.Recognition.RecognitionResult result = stt_sre.Recognize();

                if (result != null)
                {
                    //Speech result is set to the string of the result to be used in speech to text
                    speechResult = result.Text;
                    //Removes any spaces to prevent inconsistencies
                    speechResult.Replace(",", "");
                    Console.WriteLine("The result is: " + speechResult);
                    try
                    {
                        //Used in passing results from speech to text to other classes
                        speechInputCheck(speechResult);
                    }
                    catch (NullReferenceException null_ref)
                    {
                        //Used in catching if the speechResult is null, showing that the exception has been thrown and its source
                        Console.WriteLine("NullReferenceException thrown in speech: " + null_ref.Message + null_ref.Source);
                    }

                }
            }
            catch (InvalidOperationException invalid)
            {
                Console.WriteLine("InvalidOperationException in speech: " + invalid.Message + invalid.Source);
            }
            finally
            {
                //Unloads the speech recognition engine once finished
                stt_sre.UnloadAllGrammars();
            }
        }

        public RecognizerInfo kinectRecogniser()
        {
            //Creates a function using the recongiser information and a boolean
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                //Tries to get the speech result for speech recognition
                String value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                //Sets the culture to be English-US
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            //Returns the currently installed language recognition
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private void audioListen()
        {
            //This creates an audio stream to listen for the specific speech commands
            var audioSource = kinect.sensor.AudioSource;
            //Does not manipulate gain from the audio source to keep the audio input consistent
            audioSource.AutomaticGainControlEnabled = false;
            //Creates audio stream and starts it
            Stream aStream = audioSource.Start();
            //Alters the input to accept audio, allows for voice to be recognised when this method is called
            sre.SetInputToAudioStream(aStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm,
                16000, 16, 1, 32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void speechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            //Handles all of the commands when they are recognised in people's speech
            //Used in speech to text confirmation
            Boolean confirm = false;
            //Converts the speech detection result to lower case for cosnsitency
            String result = e.Result.Text.ToLower();
            //Stores the confidence of the result of the current speech detection as a double
            double conf = e.Result.Confidence;
            //Ensures that the confidence for the result is high enough and that the result is not null to prevent null exception errors
            if (conf >= 0.75 && result != null)
            {
                Console.WriteLine("Speech detection result: " + result);
                switch (result)
                {
                    case "save":
                        //Allows for saving the current program, but only whilst in the code window
                        if (kinect.currentWindow == "main")
                        {
                            events.savePress();
                        }
                        break;

                    case "run":
                        //Allows for running of the code whilst in the code window
                        if (kinect.currentWindow == "main")
                        {
                            events.runPress();
                        }
                        break;

                    case "switch":
                        //If the right hand is currently being tracked...
                        if (Settings.rightHandTracked)
                        {
                            //... sets the left hand to be tracked
                            Settings.rightHandTracked = false;
                            //Removes the previously drawn cursor from the screen
                            kinect.removePerson(kinect.People[0]);
                            kinect.removePerson(kinect.People[1]);
                            //And then re-initialises the hand skeleton
                            kinect.initialisePeople();
                        }
                        //Else if the right hand is not being tracked...
                        else if (!Settings.rightHandTracked)
                        {
                            //... sets the right hand to be tracked
                            Settings.rightHandTracked = true;
                            //And performs the same function as switching to the left hand
                            kinect.removePerson(kinect.People[0]);
                            kinect.removePerson(kinect.People[1]);
                            kinect.initialisePeople();
                        }
                        break;

                    case "mute":
                    case "quiet":
                        //Stops the text to speech from making any noise
                        Settings.ttsVolume = 0;
                        break;
                    case "unmute":
                        Settings.ttsVolume = 100;
                        break;
                    case "select":
                        //Runs the method for selecting the currently hovered icon with speech rather than a push gesture
                        speechSelect();
                        break;

                    case "undo":
                        //Undoes the previous entry into the code window, checking if the current window is the code window
                        if (kinect.currentWindow == "main")
                        {
                            events.removeLastStatement();
                        }
                        break;

                    case "start text":
                        //Checks to see if the user is in the input window
                        if (kinect.currentWindow == "input")
                        {
                            //Runs the speech to text method using the current commands result and the current confirm boolean
                            speechToText(result, confirm);
                        }
                        break;

                    case "back":
                        //Checks the current window to know where to navigate back to
                        //Potential for improvement - acquire previous window and set window content back to previous window when back is said
                        switch (kinect.currentWindow)
                        {
                            case "input":
                                kinect.openCode();
                                events.removeLastStatement();
                                break;

                            case "menu":
                                //Used to prevent the users from going back in the menu
                                kinect.frame.textToSpeech("You are already in the main window");
                                break;

                            default:
                                kinect.openMenu();
                                break;
                        }
                        break;

                    case "confirm":
                        //If the current window is input, allows for confirming of the current statement using speech
                        if (kinect.currentWindow == "input")
                        {
                            events.addStatement();
                        }
                        break;
                    case "decline":
                        //Or if the user wishes to decline the current input, that can be done using speech
                        if (kinect.currentWindow == "input")
                        {
                            //Removing the previous empty statement and opening the code window again
                            events.removeLastStatement();
                            kinect.openCode();
                        }
                        break;

                    case "set":
                    case "value":
                        //Allows the user to use speech to text to enter values for variable names and values
                        if (kinect.currentWindow == "input")
                        {
                            //varResult is used in checking which part of the variable is being set 
                            //...as well as which speech to text command is being run in the relevant method
                            varResult = result;
                            speechToText(result, confirm);
                        }
                        break;

                    case "change title":
                        //Changes the title of the current project using speech to text
                        if (kinect.currentWindow == "main")
                        {
                            //Sets varResult to be used as a check when running the speech to text commands
                            varResult = result;
                            speechToText(result, confirm);
                        }
                        break;

                    case "close":
                        //If the current window is either the menu or the main window...
                        if (kinect.currentWindow == "main" || kinect.currentWindow == "menu")
                        {
                            try
                            {
                                //Saying close closes the current running Python window
                                //Both pages allow for closing due to the open function in the menu allowing for opening of projects from the menu
                                events.cmd.Kill();
                            }
                            catch (InvalidOperationException e2)
                            {
                                //Prints the invalid operation exception if thrown, caused by the command trying to be killed if it isn't open
                                Console.WriteLine("InvalidOperationException in speech: " + e2.Message + e2.Source);
                            }
                        }
                        break;
                    //Opens either settings or help depending on which command is used, allowing for navigation using speech
                    case "help":
                        kinect.openHelp();
                        break;
                    case "settings":
                        kinect.openSettings();
                        break;
                    default:
                        //Otherwise, if no command is recognised the user is informed about this
                        kinect.frame.textToSpeech("Difficulty understanding command, please repeat");
                        Console.WriteLine("Difficulty understanding command, please repeat");
                        break;
                }
            }
        }

        public Choices getChoices()
        {
            //Gets the commands for the speech recognition
            var choices = new Choices();
            //Uses the directory of the text file allowing for portability, prevents having to enter specific directory on every new device
            //Creates a string array and stores each line of choices.txt as an element
            String[] lines = File.ReadAllLines(Settings.resourcePath + "choices.txt");
            //For every choice, each choice being a line in the file, the choices are added to the valid commands
            foreach (String line in lines)
            {
                Console.WriteLine("Adding choice: " + line);
                choices.Add(line);
            }
            //Returns the commands for use in the Microsoft.Speech Speech Recognition Engine
            return choices;
        }

        public void speechToText(String result, Boolean confirm)
        {
            //The timer is used to set a limit on how long the program waits for the user to give a speech to text result
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Checks that the time elapsed is less than five seconds and no word has been confirmed
            while (timer.Elapsed.TotalSeconds < 5 && !confirm)
            {
                //If the speech to text result is not an empty string...
                if (result != "")
                {
                    //Stops the timer
                    confirm = true;
                    timer.Stop();
                }
                //And runs the speech to text engine
                speechToTextInit();
            }
        }

        private void speechInputCheck(String speechResult)
        {
            Console.WriteLine("Setting speech result");
            //Uses the previously set varResult variable to check which speech to text method to run
            switch (varResult)
            {
                case "":
                    //When nothing is set in varResult this is used to set the text of the relevant text boxes for the input pages
                    kinect.input.txt_condVal.Text = speechResult;
                    kinect.input.txt_print.Text = speechResult;
                    //Opacity is used to match the setting of the text in the input page, since it uses reduced opacity to attain a greyed out effect
                    kinect.input.txt_condVal.Opacity = 1;
                    kinect.input.txt_print.Opacity = 1;
                    //Sets the speechResult in the input page so it is known by the program
                    InputPage.setSpeechResult(speechResult);
                    break;

                case "set":
                    //Gets first word of speech result by splitting the string on spaces and using the .First() command
                    speechResult = speechResult.Split(' ').First();
                    Console.WriteLine("Set split speechResult: " + speechResult);
                    kinect.input.txt_varSet.Opacity = 1;
                    kinect.input.txt_varSet.Text = speechResult;
                    //Sets the variable name in the input page
                    InputPage.setVarResult(speechResult);
                    break;

                case "value":
                    kinect.input.txt_varVal.Opacity = 1;
                    kinect.input.txt_varVal.Text = speechResult;
                    //Sets the value of the current variable in the input page
                    InputPage.valVarResult(speechResult);
                    break;

                case "change title":
                    //Sets the content of the project title label in the code window to be the result from the 'change title' speech to text
                    kinect.code.lbl_title.Content = speechResult;
                    Console.WriteLine("Project title: " + kinect.code.lbl_title.Content);
                    break;

                case "open page":
                    Console.WriteLine(speechResult);
                    //Opens a given file provided the user gives a correct name
                    kinect.frame.textToSpeech("Opening " + speechResult);
                    kinect.gst.events.runPress(speechResult);
                    break;
            }
            //And then resets varResult...
            varResult = "";
        }

        public void setActiveButton(Button activeBtn)
        {
            //Sets the current button being used
            currentBtn = activeBtn;
        }

        private void speechSelect()
        {
            //This method is used when selecting buttons using the 'select' speech command
            Console.WriteLine("Current button in speech: " + currentBtn.Content.ToString());
            //Reads the content of the current button...
            switch (currentBtn.Content.ToString())
            {
                //... and if it is one of the statements it runs the specific event
                case "Variable (4)":
                case "If (1)":
                case "Print (2)":
                case "Expression (5)":
                case "Loop (3)":
                    //If a statement, use the button released using the current position of the hand
                    events.buttonReleased(kinect.People[0].jHandRight.X, kinect.People[0].jHandRight.Y, currentBtn);
                    break;
                default:
                    //If not a statement, its a click event
                    events.clickHandler(currentBtn);
                    break;
            }
        }
    }
}
