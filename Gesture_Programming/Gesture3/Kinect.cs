using Microsoft.Kinect;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Gesture3
{
    //This class handles the Kinect/skeleton tracking aspect of the project
    class Kinect
    {
        #region variables

        //Instances of each of the class that the kinect needs
        public Gesture gst;
        public MainWindow frame;
        public SettingsPage settings;
        public HelpPage help;
        public Menu menu;
        public InputPage input;
        public CodeWindow code = null;
        public Speech speech;

        //Kinect values used for skeleton tracking
        public KinectSensor sensor;
        private Skeleton[] allSkeles = new Skeleton[6];
        static int trackedSkeles = 2;
        public Person[] People = new Person[trackedSkeles];

        //What the currently visible window is
        public String currentWindow = "";
        
        //The 3 types of interactable elements of the program
        public List<Button> validButtons = new List<Button>();
        public List<ComboBox> validBoxes = new List<ComboBox>();
        public List<ComboBoxItem> validBoxItems = new List<ComboBoxItem>();
        #endregion


        //Constructor for the kinect class, initiates all the required methods at the start
        public Kinect(MainWindow frame)
        {
            //Store the instance of the main window so other pages can be set as its content
            this.frame = frame;
            //Make instances of the gesture and speech classes, giving the kinect as a parameter so they can access its methods
            gst = new Gesture(this);
            speech = new Speech(this);
        }

        #region window management
        //Method to set the string variable so the program knows what window is open
        public void setActiveWindow(String activeWindow)
        {
            currentWindow = activeWindow;
            Console.WriteLine("Current window in Kinect: " + currentWindow);
        }

        //Method to make the main menu visible and interactable
        public void openMenu()
        {
            //Make a new instance of the menu
            menu = new Menu();
            //Position and size each of its elements
            menu.init();
            //Tell the program this is now the active window
            setActiveWindow("menu");
            //Get all of the interactable buttons as a list from the menu class
            validButtons = new List<Button>();
            validButtons = menu.getInteractableElements();
            //Tell the kinect to draw on the menu page
            initialisePeople();
            //Set the MainWindow to display the menu
            frame.Content = menu;
            //Reset any lingering interactable combo boxes from the input page
            validBoxes = new List<ComboBox>();
            validBoxItems = new List<ComboBoxItem>();
        }

        //Method to make the settings page visible and interactable
        public void openSettings()
        {
            //Make a new instance of the settings page
            settings = new SettingsPage();
            //Position and size each of its elements
            settings.init();
            //Set it as the active window
            setActiveWindow("settings");
            //Get the interactable buttons from its class as a list
            validButtons = new List<Button>();
            validButtons = settings.getInteractableElements();
            //Tell the kinect to draw the hand on this page
            initialisePeople();
            //Set this page as the window content
            frame.Content = settings;
            //Reset any lingering interactable combo boxes from the input page
            validBoxes = new List<ComboBox>();
            validBoxItems = new List<ComboBoxItem>();
        }

        //Method to make the help page visible and interactable
        public void openHelp()
        {
            //Make a new instance of the help page
            help = new HelpPage();
            //Position and size each of its elements
            help.init();
            //Set it as the active window
            setActiveWindow("help");
            //Get the buttons from its class as a list
            validButtons = new List<Button>();
            validButtons = help.getInteractableElements();
            //Tell the kinect to draw the hand on this page
            initialisePeople();
            //Set the page as the window content
            frame.Content = help;
            //Reset any lingering interactable combo boxes from the input page
            validBoxes = new List<ComboBox>();
            validBoxItems = new List<ComboBoxItem>();
        }

        //Method to open the code page
        public void openCode()
        {
            //Only make a new instance of the page if none exist
            //This prevents the user losing their progress if they go to a different page
            if (code == null)
            {
                code = new CodeWindow();
                //Position and size each element
                code.init();
            }
            
            //Set it as the active page
            setActiveWindow("main");
            //Remove all people from it
            removePerson(People[0]);
            removePerson(People[1]);
            //Get all interactable buttons from the page as a list
            validButtons = new List<Button>();
            validButtons = code.getInteractableElements();
            //Redraw the hands on this page
            initialisePeople();
            //Set this page as the window content
            frame.Content = code;
            //Reset any lingering interactable combo boxes from the input page
            validBoxes = new List<ComboBox>();
            validBoxItems = new List<ComboBoxItem>();
        }

        //Method to open the input page with a specific grid using the parameter
        public void openInput(String name)
        {          
            //Make a new instance of the page
            input = new InputPage();
            Console.WriteLine("Opening input window...");
            //Position and size each element
            input.init();
            //Set the specific grid as visible based on the parameter name
            input.openGrid(name);
            //If the page needs combo boxes as well as buttons for interaction, get them
            //Only the variables grid does not have a combo box which is why it is excluded
            if (name != "Variable (4)")
            {
                //Get the variabel names from the code window
                input.populateVariables(gst.getVariables());
                //Get the boxes and items as seperate lists
                validBoxes = input.getComboBoxElements();
                validBoxItems = input.items;
            }
            
            //Set this as the active window
            setActiveWindow("input");
            //Get the interactable buttons
            validButtons = new List<Button>();
            //Hard coded solution to issue where the menu buttons were being loaded along with the inputs
            validButtons.Remove(menu.btn_quit);
            validButtons = input.getInteractableElements();
            //Tell the kinect to draw on this page
            initialisePeople();
            //Set it as the window content
            frame.Content = input;
        }
        #endregion

        //Method to close the kinect
        public void closingKinect()
        {
            //Closes the kinect once the program closes
            sensor.Stop();
        }

        //Method to initiate the kinect when the program is first loaded
        public void initiate_kinect()
        {
            try
            {   //Check there is at least 1 kinect registered
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    sensor = KinectSensor.KinectSensors[0];
                }

                if (sensor.Status == KinectStatus.Connected)
                {
                    //All relevent streams for the sensor are started
                    sensor.ColorStream.Enable();
                    sensor.DepthStream.Enable();
                    sensor.SkeletonStream.Enable();

                    //Starts the sensor with a method as the event handler
                    sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
                    sensor.Start();

                    //Initialises the speech class again
                    speech = new Speech(this);
                    Console.WriteLine("Initialising speech");
                    speech.speechInit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception initialising Kinect: " + ex);
            }
        }

        //Event handler called when the kinect updates
        private void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            //tracks multiple people in loop
            for (int i = 0; i < trackedSkeles; i++)
            {
                Skeleton me = null;

                getSkeles(e, ref me, i);
                //If the skeleton object is null, return out of statement
                if (me == null)
                {
                    //If there is was someone on screen, remove them
                    if (People[i].skeleCheck == true)
                    {
                        removePerson(People[i]);
                    }
                    return;
                }
                else
                {
                    //If there was no one on screen but there is this tick, add them
                    if (People[i].skeleCheck == false)
                    {
                        addPerson(People[i]);
                    }
                }

                //sets values to people array then draws the person
                GetXYD(me, People[i]);
                People[i].DrawPerson();

                //Adds the input combo box values to the grid if that is the active window
                //Solution to a bug with the combo boxes
                if (currentWindow == "input") {
                    validBoxes = input.getComboBoxElements();
                    validBoxItems = input.items;
                    //Call the checkPersonFor method with 3 populated lists
                    gst.checkPersonFor(People[i], validButtons, validBoxes, validBoxItems);
                } else {
                    //Else, call the checkPersonFor with only the valid buttons list and 2 default values defined in the method
                    gst.checkPersonFor(People[i], validButtons, validBoxes, validBoxItems);
                }
                
            }
        }

        //Method to get the skeleton data from the kinect camera
        private void getSkeles(AllFramesReadyEventArgs e, ref Skeleton me, int person)
        {
            //retrieves skeletal data from kinect
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return;
                }
                //Inputs skeleton data into the allSKELETONS array, initialised at the top of the class
                skeletonFrameData.CopySkeletonDataTo(allSkeles);
                //query to retrive list from skeleton frame data
                List<Skeleton> tmpSkel = (from s in allSkeles where s.TrackingState == SkeletonTrackingState.Tracked select s).Distinct().ToList();
                if (tmpSkel.Count < person + 1)
                {
                    return;
                }
                //Sets me (the current person) as the current skeleton with the index of the current person
                //This ensures that only the initial people are referenced when drawing the skeletons
                me = tmpSkel[person];
            }
        }

        //Method to initialise all of the people objects
        public void initialisePeople()
        {
            for (int i = 0; i < trackedSkeles; i++)
            {
                People[i] = new Person(i);
            }
        }

        //Method to get the skeleton data and put it into an array
        private void GetSkeleton(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame();
            //Copies skeleton data into the array
            skeletonFrameData.CopySkeletonDataTo(allSkeles);
            //Sets the value of the Skeleton 'me'
            me = (from s in allSkeles
                  where s.TrackingState == SkeletonTrackingState.Tracked
                  select s).FirstOrDefault();

        }

        //Method to remove the given person parameter from the screen/page
        public void removePerson(Person person)
        {
            //Get the list of drawable joints
            List<Ellipse> Joints = person.getJoints();
            Ellipse activeHand;
            //Set the active hand circle to bea joint based off the Settings value
            if (Settings.rightHandTracked)
            {
                //This is the right hand joint
                activeHand = Joints[1];
            }
            else
            {
                //This is the left hand joint
                activeHand = Joints[0];
            }

            //Get the current window to draw this hand on
            //It removes all elements from the same canvas that the hand is drawn in (this is a different canvas than what all UI elements use)
            switch(currentWindow)
            {
                case "menu":
                    menu.cursorC.Children.RemoveRange(0,menu.cursorC.Children.Count);
                    break;

                case "settings":
                    settings.cursorC.Children.RemoveRange(0, settings.cursorC.Children.Count);
                    break;
                
                case "help":
                    help.cursorC.Children.RemoveRange(0, help.cursorC.Children.Count);
                    break;

                case "main":
                    code.cursorC.Children.RemoveRange(0, code.cursorC.Children.Count);
                    break;

                case "input":
                    input.cursorC.Children.RemoveRange(0, input.cursorC.Children.Count);
                    break;
            }
            //Tells the program that this person is no longer being drawn
            person.skeleCheck = false;
        }

        //Method to add a given persons hand to the canvas
        public void addPerson(Person person)
        {
            //Gets all drawable joints
            List<Ellipse> Joints = person.getJoints();
            Ellipse activeHand;
            //Checks which hand is being tracked
            if (Settings.rightHandTracked)
            {
                activeHand = Joints[1];
            }
            else
            {
                activeHand = Joints[0];
            }

            //Adds the tracked hand to the currently active page
            switch (currentWindow)
            {
                case "menu":
                    menu.cursorC.Children.Add(activeHand);
                    break;

                case "settings":
                    settings.cursorC.Children.Add(activeHand);
                    break;

                case "help":
                    help.cursorC.Children.Add(activeHand);
                    break;

                case "main":
                    code.cursorC.Children.Add(activeHand);
                    break;

                case "input":
                    input.cursorC.Children.Add(activeHand);
                    break;
            }
            //Tells the program this person is currently being drawn
            person.skeleCheck = true;
        }

        //Method to get the coordinates and depth data from the kinect camera and assign them to the persons joints
        private void GetXYD(Skeleton me, Person person)
        {
            //right hand
            DepthImagePoint righthandDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint righthandColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HandRight].Position, ColorImageFormat.RgbResolution640x480Fps30);
            //Upscales the joint coordinates to fit the current window size (either 640p or 720p)
            person.jHandRight.X = righthandColorPoint.X * (Settings.windowWidth/640);
            person.jHandRight.Y = righthandColorPoint.Y * (Settings.windowHeight/480);
            person.jHandRight.D = righthandDepthPoint.Depth;

            //lefthand
            DepthImagePoint lefthandDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint lefthandColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HandLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);
            //Upscales the joint coordinates to fit the current window size(either 640p or 720p)
            person.jHandLeft.X = lefthandColorPoint.X * (Settings.windowWidth / 640);
            person.jHandLeft.Y = lefthandColorPoint.Y * (Settings.windowHeight / 480);
            person.jHandLeft.D = lefthandDepthPoint.Depth;

            //Body
            DepthImagePoint bodyDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.Spine].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint bodyColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.Spine].Position, ColorImageFormat.RgbResolution640x480Fps30);
            //Upscales the joint coordinates to fit the current window size(either 640p or 720p)
            person.jSpine.X = bodyColorPoint.X * (Settings.windowWidth / 640);
            person.jSpine.Y = bodyColorPoint.Y * (Settings.windowHeight / 480);
            person.jSpine.D = bodyDepthPoint.Depth;
        }

        //Getter method to return the active window
        public String getCurrentWindow()
        {
            return currentWindow; 
        }

        //Method to close the program
        public void quitProgram()
        {
            Console.WriteLine("Active window: " + currentWindow);
            //Ensures the program can only close if the current page is the main menu
            if (currentWindow == "menu")
            {
                //Stop the kinect
                closingKinect();
                //Shut down the application
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                //Else, tell the user they cant close on this page
                Console.WriteLine("You're not in the right window to quit, silly!");
            }
        }
    }
}
