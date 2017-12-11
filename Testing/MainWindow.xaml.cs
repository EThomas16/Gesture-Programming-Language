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
using System.Threading;
using System.IO;
using System.Diagnostics;

using Microsoft.Kinect;
//using Microsoft.Speech.Recognition;
//using Microsoft.Speech.AudioFormat;


namespace CW2_Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double m_MouseX;
        double m_MouseY;

        double imports_x;
        double imports_y;
        double var_x;
        double var_y;

        int boxWidth = 180;
        int boxHeight = 23;
        int fieldHeight;

        public String explorerString = "";
        public String buttonCheck = "";

        Thickness currentMargin = new Thickness();
        String currentDrag = "";

        List<Button> imports = new List<Button>();
        List<Button> vars = new List<Button>();

        String currentEvent = "";

        Boolean buttonReleased = true;
        public Boolean windowShown = false;
        public Boolean hasSelected = false;

        int defaultButtonHeight = 34;

        public MainWindow()
        {
            //Maybe condense if possible? Not a necessity
            InitializeComponent();

            //btn_import.PreviewMouseUp += new MouseButtonEventHandler(imp_MouseUp);
            //btn_import.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(imp_MouseLeftButtonUp);
            //btn_import.PreviewMouseMove += new MouseEventHandler(btn_import_MouseMove);

            //btn_variable.PreviewMouseUp += new MouseButtonEventHandler(var_MouseUp);
            //btn_variable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(var_MouseLeftButtonUp);
            //btn_variable.PreviewMouseMove += new MouseEventHandler(btn_variable_MouseMove);

            //not needed (?)
            btn_import.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(any_MouseLeftButtonUp);
            btn_variable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(any_MouseLeftButtonUp);
            btn_import.PreviewMouseMove += new MouseEventHandler(import_MouseMove);
            btn_variable.PreviewMouseMove += new MouseEventHandler(variable_MouseMove);
            btn_import.PreviewMouseUp += new MouseButtonEventHandler(import_MouseUp);
            //btn_variable.PreviewMouseUp += new MouseButtonEventHandler(variable_MouseUp);

            imports_x = btn_import.Margin.Left;
            imports_y = btn_import.Margin.Top;

            var_x = btn_variable.Margin.Left;
            var_y = btn_variable.Margin.Top;
        }


        public void any_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_MouseX = e.GetPosition(this).X;
            m_MouseY = e.GetPosition(this).Y;

        }
        /*
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
        */


        private void import_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.MouseDevice.Capture(btn_import);
                Button temp = btn_import;
                currentMargin = btn_import.Margin;
                //Currently selected item, this being the import method import is used
                currentDrag = "import";

                any_MouseMove(temp, e);
            }
        }

        private void variable_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.MouseDevice.Capture(btn_variable);
                Button temp = btn_variable;
                currentMargin = btn_variable.Margin;
                //Currently selected item, this being the import method import is used
                currentDrag = "variable";

                any_MouseMove(temp, e);
            }
        }

        private void any_MouseMove(Button temp, MouseEventArgs e)
        {
            // Capture the mouse for border
            int _tempX = Convert.ToInt32(e.GetPosition(this).X);
            int _tempY = Convert.ToInt32(e.GetPosition(this).Y);
            // when While moving _tempX get greater than m_MouseX relative to usercontrol 
            if (m_MouseX > _tempX)
            {
                // add the difference of both to Left
                currentMargin.Left += (_tempX - m_MouseX);
                // subtract the difference of both to Left
                currentMargin.Right -= (_tempX - m_MouseX);
            }
            else
            {
                currentMargin.Left -= (m_MouseX - _tempX);
                currentMargin.Right -= (_tempX - m_MouseX);
            }
            if (m_MouseY > _tempY)
            {
                currentMargin.Top += (_tempY - m_MouseY);
                currentMargin.Bottom -= (_tempY - m_MouseY);
            }
            else
            {
                currentMargin.Top -= (m_MouseY - _tempY);
                currentMargin.Bottom -= (_tempY - m_MouseY);
            }
            temp.Margin = currentMargin;
            m_MouseX = _tempX;
            m_MouseY = _tempY;

        }

        private void import_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = importBorder;
            currentDrag = "import";
            any_MouseUp(e, currentBorder);
        }

        private void any_MouseUp(MouseButtonEventArgs e, Border currentBorder)
        {
            e.MouseDevice.Capture(null);

            double borderLeft = currentBorder.Margin.Left;
            double borderTop = currentBorder.Margin.Top;
            double borderRight = borderLeft + currentBorder.ActualWidth;
            double borderBottom = borderTop + currentBorder.ActualHeight;

            Console.WriteLine("1");

            if (m_MouseY > borderTop && m_MouseY < borderBottom && m_MouseX > borderLeft && m_MouseX < borderRight && currentDrag == "import")
            {
                Console.WriteLine("2");
                Button currentStatement = new Button();
                importBounds.Children.Add(currentStatement);
                Grid.SetRow(currentStatement, importBounds.RowDefinitions.Count);
                currentStatement.Name = "import" + imports.Count;
                imports.Add(currentStatement);
                currentBorder.Height += defaultButtonHeight * 1.8;
                currentStatement.Height = defaultButtonHeight;
                currentStatement.Width = currentBorder.Width;
                currentStatement.Content = "EMPTY";
                double left = borderLeft - borderRight + (currentStatement.Width);
                double top = -1;
                if (imports.Count > 1)
                {
                    top = imports[imports.Count - 2].Margin.Top - 15;
                }
                else
                {
                    top = (importBounds.Height) - importBounds.Height * 1.5;
                }

                setButtonLocation(currentStatement, left, top);
                Form1 importExplorer = new Form1(this, currentStatement, "");
                importExplorer.Show();

                foreach (FrameworkElement element in importBounds.Children)
                {
                    //try { }
                    Console.WriteLine(element);
                }

                importBounds.RowDefinitions.Add(new RowDefinition());

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
            }
            defaultStatementLocations();
        }


        public void setButtonLocation(Button current, double x, double y, double right = -1, double bottom = -1)
        {
            Thickness _margin = new Thickness();
            _margin = current.Margin; //Useless?
            _margin.Left = x;
            _margin.Top = y;
            if (right != -1)
            {
                _margin.Right = right;
                _margin.Bottom = bottom;
            }
            current.Margin = _margin;
        }

        //private SpeechRecognitionEngine sre;
        //private Thread audioThread;

        //Grid Details -- Allows users to generate their own grid MATH MUST BE EQUAL TO SIZE OF CAMERA INPUT
        int ROW_COUNT = 4;
        int COLUMN_COUNT = 5;
        //The border around the grid is used to provide an outline to the image
        //These values need to be taken into account when performing calculations on the grid
        double BORDER_TOP = 10;
        double BORDER_LEFT = 5;
        double BORDER_BOTTOM = 10;
        double BORDER_RIGHT = 5;

        //Initialises the variables for the size of the entire grid, as well as each grid element
        private double BOX_WIDTH;
        private double BOX_HEIGHT;
        private const double HEIGHT = 480;
        private const double WIDTH = 640;
        //Used to check if there is a rectangle currently displaying, if false a new rectangle can be generated
        Boolean recDisplaying = false;

        int rightArmState = -1;
        int leftArmState = -1;
        int rightLegState = -1;
        int leftLegState = -1;

        int currentRow = 0;
        int currentCol = 0;

        Boolean rWaveSegOne = false;
        Boolean rWaveSegTwo = false;
        Boolean rWaveSegThree = false;
        Boolean rWaveSegFour = false;

        Boolean lWaveSegOne = false;
        Boolean lWaveSegTwo = false;
        Boolean lWaveSegThree = false;
        Boolean lWaveSegFour = false;

        Boolean seated = false;
        Boolean notSeated = true;

        int gestureFrames = 100;

        //Array of numbers to be used in grid selection, declared as full strings due to method of interpretation by the speech recognition engine
        String[] numberList = new String[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty"};

        //Skeleton tracking variables are set here, and all skeletons are passed into an array of size SKELETON_COUNT
        private KinectSensor sensor;
        private const int SKELETON_COUNT = 6;
        private Skeleton[] allSKELETONS = new Skeleton[SKELETON_COUNT];

        //Skeletons tracked is not a constant, but is set as two due to the limitations of the Kinect v1
        static int SKELETONS_TRACKED = 2;
        Person[] People = new Person[SKELETONS_TRACKED];

        //Manages the enabling and disabling of video output
        private Boolean SHOW_VIDEO = true;

        private void frmKinectInterface_Loading(object sender, RoutedEventArgs e)
        {

            //Initialises all of the required methods to start the Kinect application
            this.initiate_kinect();
            //Build grid here
            //buildGrid();
        }

        private void frmKinectInterface_Colsing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //dispose of sensors
            sensor.Stop();
        }

        private void initiate_kinect()
        {
            try
            {
                //checking for multiple kinect sensors and accessing first one
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    sensor = KinectSensor.KinectSensors[0];
                }
                //Check status of kinect, if it's connected then the code is run
                if (sensor.Status == KinectStatus.Connected)
                {
                    sensor.ColorStream.Enable();
                    sensor.DepthStream.Enable();
                    sensor.SkeletonStream.Enable();
                    ////WINDOWS KINECT ONLY!!!!! (NEAR MODE)
                    //sensor.DepthStream.Range = DepthRange.Near;
                    //sensor.SkeletonStream.EnableTrackingInNearRange = true; 
                    //Load People, adds joint and bone drawing to the display to show tracking
                    initializePeople();
                    //Handles initialisation of the video and starts the sensor
                    sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
                    sensor.Start();
                    //Voice recognition initialised and grid built after creating the video window instance
                    //Prevents issues with displaying the grid
                    //buildGrid();
                    Debug.WriteLine("speech initialised");
                    //initializeSpeech();
                }
                //Catches any exceptions with the initialisation and displays the relevant error
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception initialising Kinect, exception: " + ex);
            }

        }

        private void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            //Retrieves source from kinect sensor
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }
                //Byte stream of image to get size of the display window
                byte[] pixels = new byte[colorFrame.PixelDataLength];
                //Copies a frame of colour data to the byte array, stores this to be used when creating the video stream
                colorFrame.CopyPixelDataTo(pixels);

                int stride = colorFrame.Width * 4;

                //displays video
                //Uses the byte array's width and height, as well as giving a pixel format, to display the video information accordingly
                if (SHOW_VIDEO)
                {
                    //this.imgVideo.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
                }
            }

            //tracks multiple people in loop
            for (int i = 0; i < SKELETONS_TRACKED; i++)
            {

                Skeleton me = null;

                GetSKELETONS(e, ref me, i);
                //If the skeleton object is null, return out of statement
                if (me == null)
                {
                    if (People[i].skeleCheck == true)
                    {
                        removePerson(People[i]);
                    }
                    return;
                }
                else if (me != null)
                {
                    if (People[i].skeleCheck == false)
                    {
                        addPerson(People[i]);
                    }
                }

                if (People[i].startingDepth != -1)
                {
                    People[i].startingDepth = People[i].jHead.D;
                }

                //sets values to people array then draws the person
                GetXYD(me, People[i]);
                People[i].DrawPerson();

                //things to check for i.e. gestures grid ...
                //Checks if there is a person there (skeleton being drawn) and handles push gesture
                this.checkPersonFor(People[i]);

                //things to check for i.e. gestures grid...
                //put setSelectedTile and setHoveredTile here
                //for (int j = 0; j < SKELETONS_TRACKED; j++) {
                //    setHoveredTile(People[j]);
                //}

            }

        }

        private void GetSKELETONS(AllFramesReadyEventArgs e, ref Skeleton me, int person)
        {
            //retrieves skeletal data from kinect
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return;
                }
                //Inputs skeleton data into the allSKELETONS array, initialised at the top of the class
                skeletonFrameData.CopySkeletonDataTo(allSKELETONS);
                //query to retrive list from skeleton frame data
                List<Skeleton> tmpSkel = (from s in allSKELETONS where s.TrackingState == SkeletonTrackingState.Tracked select s).Distinct().ToList();
                if (tmpSkel.Count < person + 1)
                {
                    return;
                }
                //Sets me (the current person) as the current skeleton with the index of the current person
                //This ensures that only the initial people are referenced when drawing the skeletons
                me = tmpSkel[person];
            }
        }

        private void initializePeople()
        {
            for (int i = 0; i < SKELETONS_TRACKED; i++)
            {
                //People array initialised as Person object
                People[i] = new Person(i);
                People[i].skeleCheck = true;
                //Indexes using People[i] to only reference the current person, determined by the number of skeletons being tracked
                List<Ellipse> Joints = People[i].getJoints();
                //adds joints to display
                foreach (Ellipse Joint in Joints)
                {
                    //Adds the joints to the canDraw class in the .xaml
                    canDraw.Children.Add(Joint);
                }
                //adds limbs to display
                List<Line> Bones = People[i].getBones();
                foreach (Line Bone in Bones)
                {
                    canDraw.Children.Add(Bone);
                }

                List<Rectangle> rectangles = People[i].getAdditional();
                foreach (Rectangle rect in rectangles)
                {
                    canDraw.Children.Add(rect);
                }
            }
        }

        private void GetSkeleton(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame();
            //Copies skeleton data into the array
            skeletonFrameData.CopySkeletonDataTo(allSKELETONS);
            //Sets the value of the Skeleton 'me'
            me = (from s in allSKELETONS
                  where s.TrackingState == SkeletonTrackingState.Tracked
                  select s).FirstOrDefault();

        }

        private void removePerson(Person person)
        {
            //Acquires the joints being draw to remove them
            List<Ellipse> Joints = person.getJoints();

            foreach (Ellipse Joint in Joints)
            {
                canDraw.Children.Remove(Joint);
            }
            //Same process is repeated for bones as for joints
            List<Line> Bones = person.getBones();

            foreach (Line Bone in Bones)
            {
                canDraw.Children.Remove(Bone);
            }
            //Denotes that the skeleton has now been removed so it can always be re-added
            person.skeleCheck = false;
        }

        private void addPerson(Person person)
        {
            List<Ellipse> Joints = person.getJoints();
            //Loops to add all of the joints for the skeleton from the object
            foreach (Ellipse Joint in Joints)
            {
                canDraw.Children.Add(Joint);
            }
            List<Line> Bones = person.getBones();
            //Same process is repeated for bones as for joints
            foreach (Line Bone in Bones)
            {
                canDraw.Children.Add(Bone);
            }
            //Skeleton check is made true as a skeleton has been added to the output window
            person.skeleCheck = true;
        }

        private void GetXYD(Skeleton me, Person person)
        {
            //head
            DepthImagePoint headDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint
            (me.Joints[JointType.Head].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint headColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint
            (me.Joints[JointType.Head].Position, ColorImageFormat.RgbResolution640x480Fps30);
            person.jHead.X = headColorPoint.X;
            person.jHead.Y = headColorPoint.Y;
            person.jHead.D = headDepthPoint.Depth;

            //This method is responsible for setting all of the drawing elements for each joint and bone, ensures they are displayed on the screen

            //right hand
            DepthImagePoint righthandDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint righthandColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HandRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jHandRight.X = righthandColorPoint.X;
            person.jHandRight.Y = righthandColorPoint.Y;
            person.jHandRight.D = righthandDepthPoint.Depth;

            //leftelbow
            DepthImagePoint rightelbowDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.ElbowRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint rightelbowColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.ElbowRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jElbowRight.X = rightelbowColorPoint.X;
            person.jElbowRight.Y = rightelbowColorPoint.Y;
            person.jElbowRight.D = rightelbowDepthPoint.Depth;

            //lefthand
            DepthImagePoint lefthandDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint lefthandColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HandLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jHandLeft.X = lefthandColorPoint.X;
            person.jHandLeft.Y = lefthandColorPoint.Y;
            person.jHandLeft.D = lefthandDepthPoint.Depth;

            //leftelbow
            DepthImagePoint leftelbowDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.ElbowLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint leftelbowColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.ElbowLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jElbowLeft.X = leftelbowColorPoint.X;
            person.jElbowLeft.Y = leftelbowColorPoint.Y;
            person.jElbowLeft.D = leftelbowDepthPoint.Depth;

            //Body
            DepthImagePoint bodyDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.Spine].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint bodyColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.Spine].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jSpine.X = bodyColorPoint.X;
            person.jSpine.Y = bodyColorPoint.Y;
            person.jSpine.D = bodyDepthPoint.Depth;

            //Hip Left
            DepthImagePoint hipLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HipLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint hipLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HipLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jHipLeft.X = hipLeftColorPoint.X;
            person.jHipLeft.Y = hipLeftColorPoint.Y;
            person.jHipLeft.D = hipLeftDepthPoint.Depth;

            //hip right
            DepthImagePoint hipRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HipRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint hipRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HipRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jHipRight.X = hipRightColorPoint.X;
            person.jHipRight.Y = hipRightColorPoint.Y;
            person.jHipRight.D = hipRightDepthPoint.Depth;
            //foot left
            DepthImagePoint FootLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.FootLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint FootLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.FootLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jFootLeft.X = FootLeftColorPoint.X;
            person.jFootLeft.Y = FootLeftColorPoint.Y;
            person.jFootLeft.D = FootLeftDepthPoint.Depth;
            //foot right
            DepthImagePoint FootRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.FootRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint FootRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.FootRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jFootRight.X = FootRightColorPoint.X;
            person.jFootRight.Y = FootRightColorPoint.Y;
            person.jFootRight.D = FootRightDepthPoint.Depth;
            //hip center
            DepthImagePoint HipCenterDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HipCenter].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint HipCenterColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.HipCenter].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jHipCenter.X = HipCenterColorPoint.X;
            person.jHipCenter.Y = HipCenterColorPoint.Y;
            person.jHipCenter.D = HipCenterDepthPoint.Depth;
            //KneeRight
            DepthImagePoint KneeRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.KneeRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint KneeRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.KneeRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jKneeRight.X = KneeRightColorPoint.X;
            person.jKneeRight.Y = KneeRightColorPoint.Y;
            person.jKneeRight.D = KneeRightDepthPoint.Depth;
            //KneeLeft
            DepthImagePoint KneeLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.KneeLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint KneeLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.KneeLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jKneeLeft.X = KneeLeftColorPoint.X;
            person.jKneeLeft.Y = KneeLeftColorPoint.Y;
            person.jKneeLeft.D = KneeLeftDepthPoint.Depth;
            //ShoulderCenter
            DepthImagePoint ShoulderCenterDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.ShoulderCenter].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint ShoulderCenterColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.ShoulderCenter].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jShoulderCenter.X = ShoulderCenterColorPoint.X;
            person.jShoulderCenter.Y = ShoulderCenterColorPoint.Y;
            person.jShoulderCenter.D = ShoulderCenterDepthPoint.Depth;
            //ShoulderLeft
            DepthImagePoint ShoulderLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.ShoulderLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint ShoulderLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.ShoulderLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jShoulderLeft.X = ShoulderLeftColorPoint.X;
            person.jShoulderLeft.Y = ShoulderLeftColorPoint.Y;
            person.jShoulderLeft.D = ShoulderLeftDepthPoint.Depth;
            //ShoulderRight
            DepthImagePoint ShoulderRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.ShoulderRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint ShoulderRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.ShoulderRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jShoulderRight.X = ShoulderRightColorPoint.X;
            person.jShoulderRight.Y = ShoulderRightColorPoint.Y;
            person.jShoulderRight.D = ShoulderRightDepthPoint.Depth;
            //WristLeft
            DepthImagePoint WristLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.WristLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint WristLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.WristLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jWristLeft.X = WristLeftColorPoint.X;
            person.jWristLeft.Y = WristLeftColorPoint.Y;
            person.jWristLeft.D = WristLeftDepthPoint.Depth;
            //WristRight
            DepthImagePoint WristRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.WristRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint WristRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.WristRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jWristRight.X = WristRightColorPoint.X;
            person.jWristRight.Y = WristRightColorPoint.Y;
            person.jWristRight.D = WristRightDepthPoint.Depth;
            //AnkleLeft
            DepthImagePoint AnkleLeftDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.AnkleLeft].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint AnkleLeftColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.AnkleLeft].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jAnkleLeft.X = AnkleLeftColorPoint.X;
            person.jAnkleLeft.Y = AnkleLeftColorPoint.Y;
            person.jAnkleLeft.D = AnkleLeftDepthPoint.Depth;
            //AnkleRight
            DepthImagePoint AnkleRightDepthPoint = sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.AnkleRight].Position, DepthImageFormat.Resolution640x480Fps30);
            ColorImagePoint AnkleRightColorPoint = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(me.Joints[JointType.AnkleRight].Position, ColorImageFormat.RgbResolution640x480Fps30);

            person.jAnkleRight.X = AnkleRightColorPoint.X;
            person.jAnkleRight.Y = AnkleRightColorPoint.Y;
            person.jAnkleRight.D = AnkleRightDepthPoint.Depth;

        }

        /*private void initializeSpeech()
        {
            //Acquires the speech recogniser object and makes an instance of it
            RecognizerInfo ri = GetKinectRecognizer();
            sre = new SpeechRecognitionEngine(ri.Id);
            //Acquires the list of commands that can be used with the speech recognition engine
            var commands = getChoices();
            //Allows support for different languages
            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(commands);
            //Sets the grammar to be interpreted, in this case English
            var g = new Grammar(gb);
            //Loads the grammar into the speech recognition engine and creates a new event handler for when speech commands are triggered
            sre.LoadGrammar(g);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Kinect_SpeechRecognized);
            //Creates a thread to handle audio input using the startAudioListening method
            audioThread = new Thread(startAudioListening);
            audioThread.Start();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            //Initialisation of the speech recognition engine
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name,
                StringComparison.InvariantCultureIgnoreCase);
            };
            return
            SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();


        }

        private void startAudioListening()
        {
            var audioSource = sensor.AudioSource;
            audioSource.AutomaticGainControlEnabled = false;
            //Creates audio stream and starts it
            Stream aStream = audioSource.Start();
            //Alters the input to accept audio, allows for voice to be recognised when this method is called
            sre.SetInputToAudioStream(aStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm,
                16000, 16, 1, 32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }
        //-----------------------------Methods to handle speech recognition-----------------------------
        public void Kinect_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String resultStr = e.Result.Text.ToLower();
            String resultNum = resultStr.Substring(0, Math.Min(resultStr.Length, 6));
            double conf = e.Result.Confidence;

            currentRow = People[0].selectedRow;
            currentCol = People[0].selectedColumn;

            Debug.WriteLine("You said: " + resultStr);

            if (conf >= 0.8 && resultStr == "test")
            {
                Debug.WriteLine("hello there");
            }

            //If the speech recognising engine has 80% certainty of a word, it checks the case and performs the specific function
            else if (conf >= 0.8 && resultStr == "select")
            {
                setSelectedTile(People[0]);
                Debug.WriteLine("Grid element selected");
                //Else, calculate the number the user said and highlight the grid index
            }
            else if (conf >= 0.8 && numberList.Contains(resultStr))
            {
                Debug.WriteLine("number detected, calculating");
                int resultPos = Array.IndexOf(numberList, resultStr);
                Debug.WriteLine("Calculating grid position");
                //Stores row and column in 2 index array
                int[] gridCount;
                gridCount = gridPosCalc(resultPos);
                //Adds one to position to account for indexing
                gridCount[0] = gridCount[0] + 1;
                gridCount[1] = gridCount[1] + 1;
                voiceNumber(gridCount[0], gridCount[1]);
                txtVoiceCommand.Text = "Number selected: " + numberList[resultPos];
                //Else, switch the hand being tracked when the user says 'switch'
            }
            else if (conf >= 0.8 && resultStr == "switch")
            {
                //Stores the currently tracked hand when the method is called
                Boolean tLeft = People[0].TRACK_LEFT_HAND;
                Boolean tRight = People[0].TRACK_RIGHT_HAND;
                //If the right hand is being tracked, switch the currently tracked hand
                if (tLeft == false && tRight)
                {
                    People[0].TRACK_RIGHT_HAND = false;
                    People[0].TRACK_LEFT_HAND = true;
                    Debug.WriteLine("Left hand: " + People[0].TRACK_LEFT_HAND + "Right hand: " + People[0].TRACK_RIGHT_HAND);
                    //And vice versa for the left hand
                }
                else if (tRight == false && tLeft)
                {
                    People[0].TRACK_RIGHT_HAND = true;
                    People[0].TRACK_LEFT_HAND = false;
                    Debug.WriteLine("Left hand: " + People[0].TRACK_LEFT_HAND + "Right hand: " + People[0].TRACK_RIGHT_HAND);
                }
                //Else, switch the skeleton tracking from standing to seated and vice versa
            }
            else if (conf >= 0.8 && resultStr == "toggle seated")
            { //Add a way to reset this to standing
                if (notSeated)
                {
                    notSeated = false;
                    sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    Debug.WriteLine("Sitting");
                    seated = true;
                }
                else if (seated)
                {
                    seated = false;
                    sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                    Debug.WriteLine("Standing");
                    notSeated = true;
                }
            }
            //Allows for manipulation of the currently selected grid element, where it can be moved up, down, left or right
            else if (conf >= 0.8 && resultStr == "up")
            {
                //Checks if current row is within the constraints of the grid
                if (0 < currentRow && currentRow <= ROW_COUNT)
                {
                    Debug.WriteLine("Current Row: " + currentRow);
                    //Decrements current row to move up
                    currentRow = currentRow--;
                    Debug.WriteLine("New Row: " + currentRow);
                    voiceNumber(currentRow, currentCol);
                    txtVoiceCommand.Text = "You moved the tile up";
                }
            }
            else if (conf >= 0.8 && resultStr == "left")
            {
                //Same check as before
                if (0 < currentCol && currentCol <= COLUMN_COUNT)
                {
                    //Decrements column to move left
                    currentCol = currentCol--;
                    voiceNumber(currentRow, currentCol);
                    txtVoiceCommand.Text = "You moved the tile left";
                }
            }
            else if (conf >= 0.8 && resultStr == "down")
            {
                //Same check as before
                if (0 <= currentRow && currentRow < ROW_COUNT)
                {
                    //Increments row to move down
                    Debug.WriteLine("Current Row: " + currentRow);
                    currentRow = currentRow++;
                    Debug.WriteLine("New Row: " + currentRow);
                    voiceNumber(currentRow, currentCol);
                    txtVoiceCommand.Text = "You moved the tile down";
                }
            }
            else if (conf >= 0.8 && resultStr == "right")
            {
                //Same check as before
                if (0 <= currentCol && currentCol < COLUMN_COUNT)
                {
                    //Increments column to move right
                    currentCol = currentCol++;
                    Debug.WriteLine("New Row: " + currentRow);
                    voiceNumber(currentRow, currentCol);
                    txtVoiceCommand.Text = "You moved the tile right";
                }
            }
        }

        public Choices getChoices()
        {
            //Dynamically alters the total number of numbers to be added to the options
            int numberLimit = (ROW_COUNT * COLUMN_COUNT) - 1;
            String[] speechChoices = new String[] { "test", "select", "switch", "toggle seated", "up", "down", "left", "right" };
            var choices = new Choices();

            for (int i = 0; i <= speechChoices.Length - 1; i++)
            {
                Debug.WriteLine("Choice added: " + speechChoices[i]);
                choices.Add(speechChoices[i]);
            }
            //Adds all of the grid numbers as options to the speech engine, uses values from the array of number strings
            for (int i = 0; i <= numberLimit; i++)
            {
                Debug.WriteLine("Choice added: " + numberList[i]);
                choices.Add(numberList[i]);
            }

            return choices;
        }*/

        public int[] gridPosCalc(int index)
        {
            //Locally stored to prevent conflicts
            int row = 0;
            int col = 0;
            //Column and row values are calculated from the index using the relevant operations. The row value rounds to the nearest integer
            col = index % COLUMN_COUNT;
            row = index / COLUMN_COUNT;
            //Returns a two index array of the row and column values, calculated from the index
            int[] gridCount = { row, col };
            return gridCount;
        }

        public void voiceNumber(int row, int col)
        {
            //Removes currently drawn rectangle
            canDraw.Children.Remove(People[0].recSelected);
            //Shows that the rectangle is not displaying so can be called again in setSelectedTile()
            recDisplaying = false;
            //Sets the row and column values as the parameters (from gridPosCalc usually)
            People[0].selectedRow = row;
            People[0].selectedColumn = col;
            //Sets the selected tile as the current row and column element
            //setSelectedTile(People[0]);
        }

        /*private void buildGrid()
        {
            //calculating grid
            //4x5 grid
            BOX_WIDTH = ((WIDTH - (BORDER_LEFT + BORDER_RIGHT)) / COLUMN_COUNT);
            BOX_HEIGHT = ((HEIGHT - (BORDER_TOP + BORDER_BOTTOM)) / ROW_COUNT);
            //building GUI grid
            //Debug.WriteLine("Width: " + BOX_WIDTH + "\nHeight: " + BOX_HEIGHT);
            //border rows
            RowDefinition topBorderRow = new RowDefinition();
            topBorderRow.Height = new GridLength(BORDER_TOP);
            RowDefinition bottomBorderRow = new RowDefinition();
            bottomBorderRow.Height = new GridLength(BORDER_BOTTOM);

            //border cols
            ColumnDefinition rightBorderCol = new ColumnDefinition();
            rightBorderCol.Width = new GridLength(BORDER_RIGHT);
            ColumnDefinition leftBorderCol = new ColumnDefinition();
            leftBorderCol.Width = new GridLength(BORDER_LEFT);

            //adding rows to grid
            this.grdOverlay.RowDefinitions.Add(topBorderRow);
            for (int i = 0; i < ROW_COUNT; i++)
            {
                RowDefinition defaultRow = new RowDefinition();
                defaultRow.Height = new GridLength(BOX_HEIGHT);
                this.grdOverlay.RowDefinitions.Add(defaultRow);
            }
            this.grdOverlay.RowDefinitions.Add(bottomBorderRow);

            //adding cols to grid
            this.grdOverlay.ColumnDefinitions.Add(leftBorderCol);
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                ColumnDefinition defaultCol = new ColumnDefinition();
                defaultCol.Width = new GridLength(BOX_WIDTH);
                this.grdOverlay.ColumnDefinitions.Add(defaultCol);
            }
            this.grdOverlay.ColumnDefinitions.Add(rightBorderCol);

        }

        private void setSelectedTile(Person person)
        {
            // SETTING SELECTED GRID TILE
            person.recSelected.SetValue(Grid.RowProperty, person.selectedRow);
            person.recSelected.SetValue(Grid.ColumnProperty, person.selectedColumn);
            //Displays the current row and column that have been selected on the grid
            txtSelectedTile.Text = "SELECTED row:" + person.selectedRow + "    col:" + person.selectedColumn;
            //Debug.WriteLine("Row: " + person.selectedRow + "\nColumn: " + person.selectedColumn);

            //If there is no rectangle currently being displayed, the code runs. recDisplaying is global so it can be monitored from any method in the class
            if (recDisplaying == false)
            {
                //Sets the rectangle's height and width for when it is drawn on the window
                person.recSelected.Height = BOX_HEIGHT;
                person.recSelected.Width = BOX_WIDTH;
                //Adds the rectangle to the window
                canDraw.Children.Add(person.recSelected);
                //Sets the position of the drawn items on the canvas
                Canvas.SetLeft(person.recSelected, getGridX(person.selectedColumn - 1));
                Canvas.SetTop(person.recSelected, getGridY(person.selectedRow - 1));
                //Breaks any other rectangle statements to reset
                recDisplaying = true;
            }
        }*/

        private int getGridX(int selectedRow)
        {
            //Gets the current x coordinate on the grid
            int gridX;

            gridX = (selectedRow * (int)BOX_WIDTH) + (int)BORDER_LEFT;

            return gridX;
        }

        private int getGridY(int selectedColumn)
        {
            //Gets the current y coordinate on the grid
            int gridY;

            gridY = (selectedColumn * (int)BOX_HEIGHT) + (int)BORDER_TOP;

            return gridY;
        }

        private void setHoveredTile(Person person)
        {
            //Debug.WriteLine("Hovered tile called");
            person.recHover.Height = BOX_HEIGHT;
            person.recHover.Width = BOX_WIDTH;
            person.recHover.SetValue(Grid.RowProperty, person.selectedRow);
            person.recHover.SetValue(Grid.ColumnProperty, person.selectedColumn);
            //Debug.WriteLine("Hovered tile completed\n Col: " + person.selectedColumn + "Row: " + person.selectedRow);
            Canvas.SetLeft(person.recHover, getGridX(person.selectedColumn - 1));
            Canvas.SetTop(person.recHover, getGridY(person.selectedRow - 1));
            //Debug.WriteLine("Hovered canvas element added");
        }

        private void gridCheck(Person person)
        {
            //Stores the current location of the person's right hand
            double HX = person.jHandRight.X;
            double HY = person.jHandRight.Y;
            //What row the hand is in
            for (int i = 1; i <= ROW_COUNT; i++)
            {
                if (HY <= ((BOX_HEIGHT * i) + BORDER_TOP) && HY > ((BOX_HEIGHT * (i - 1)) + BORDER_TOP))
                {
                    if (i != person.selectedRow)
                    {
                        person.selectedRow = i;
                    }
                }
            }
            //What column is the hand in
            for (int i = 1; i <= COLUMN_COUNT; i++)
            {
                if (HX <= ((BOX_WIDTH * i) + BORDER_LEFT) && HX > ((BOX_WIDTH * (i - 1)) + BORDER_LEFT))
                {
                    if (i != person.selectedColumn)
                    {
                        person.selectedColumn = i;
                    }
                }
            }
            setHoveredTile(person);
        }

        //-----------------------------Methods to handle gestures-----------------------------
        private void checkPersonFor(Person person)
        {
            //Checks for if the person performs a specific gesture. If no gesture is detected, the below string displays
            String gestureCheck = "No gesture found";
            gridCheck(person);
            //Handles the different gestures to be detected
            handlePush(person);
            handleRightArm(person, ref gestureCheck);
            handleLeftArm(person, ref gestureCheck);
            handleBothArms(person, ref gestureCheck);
            handleWalk(person, ref gestureCheck);
            handleRightLeg(person, ref gestureCheck);
            handleLeftLeg(person, ref gestureCheck);
            handleClap(person, ref gestureCheck);
            handleWave(person, ref gestureCheck);
            //Displays the current gesture to the output window, is updated every time the function loops
            //txtGesture.Text = gestureCheck;
        }
        private void handlePush(Person person)
        {
            //Manages the pushing motion to select a grid element
            double difference;
            //tracking right or left hand
            if (person.TRACK_RIGHT_HAND)
            {
                difference = person.jSpine.D - person.jHandRight.D;
            }
            else
            { //Alter this to have a check for the left hand?
                difference = person.jSpine.D - person.jHandLeft.D;
            }
            //comparisoon of spine and hand depth to determine whether they are making a pushing motion
            if (difference > person.PUSH_DIFFERANCE)
            {
                //this.setSelectedTile(person); (OLD)
                //setSelectedTile(person);
                button_Click(new object(), new RoutedEventArgs());
                button_Dragged(person, btn_import);
                buttonReleased = false;
            }
            else
            {
                //Removes the currently drawn rectangle and allows another to be drawn
                canDraw.Children.Remove(person.recSelected);
                recDisplaying = false;
                if (!buttonReleased)
                {
                    buttonReleased = true;
                    import_Release(person);
                }
            }
        }

        private void handleRightArm(Person person, ref String gestureCheck)
        {
            int rightHandX = person.jHandRight.X;
            int rightHandY = person.jHandRight.Y;
            int rightShoulderX = person.jShoulderRight.X;
            int rightShoulderY = person.jShoulderRight.Y;
            int rightElbowX = person.jElbowRight.X;
            int rightElbowY = person.jElbowRight.Y;
            //Checks if the shoulder and hand are level to see if the arm is straightened
            if ((rightShoulderY - rightHandY) <= 20 && (rightShoulderY - rightHandY) >= -20 && rightShoulderX < rightHandX)
            {
                rightArmState = 0;
                //Passes the current gesture to the gestureCheck variable to be displayed
                gestureCheck = "Right arm straightened";
                //Checks if the second segment of the wave has been detected (arm half bent) and allows the next segment to begin
                if (rWaveSegTwo != true && rWaveSegOne == true)
                {
                    rWaveSegTwo = true;
                    //Checks for the fourth segment of the arm being straightened to complete the wave
                }
                else if (rWaveSegThree == true && rWaveSegFour != true)
                {
                    rWaveSegFour = true;
                }
                //Checks if the shoulder and elbow are level as well as if the hand and elbow are aligned on the x axis, this detects when the arm is half bent
            }
            else if ((rightShoulderY - rightElbowY) <= 20 && (rightShoulderY - rightElbowY) >= -20)
            {
                if ((rightHandX - rightElbowX) <= 20 && (rightHandX - rightElbowX) >= -20)
                {
                    rightArmState = 1;
                    gestureCheck = "Right arm bent half";
                    //If the arm is bent half, then the first wave segment is set to true
                    if (rWaveSegOne != true)
                    {
                        rWaveSegOne = true;
                        //If the second segment (arm straightened) is detected and the third segment has not been detected, the third segment is set
                        //This allows for the fourth segment to be checked for in the first conditional statement above
                    }
                    else if (rWaveSegTwo == true && rWaveSegThree != true)
                    {
                        rWaveSegThree = true;
                    }
                    //If the shoulder and hand are aligned on the x axis, the arm is stated as being bent fully
                }
                else if ((rightShoulderX - rightHandX) <= 20 && (rightShoulderX - rightHandX) >= -20)
                {
                    rightArmState = 2;
                    gestureCheck = "Right arm bent fully";
                }
            }
            else
            {
                //Resets the current arm state
                rightArmState = -1;
            }

        }
        //Same principles as handling the right arm, except inverted for the left arm
        private void handleLeftArm(Person person, ref String gestureCheck)
        {
            int leftHandX = person.jHandLeft.X;
            int leftHandY = person.jHandLeft.Y;
            int leftShoulderX = person.jShoulderLeft.X;
            int leftShoulderY = person.jShoulderLeft.Y;
            int leftElbowX = person.jElbowLeft.X;
            int leftElbowY = person.jElbowLeft.Y;

            if ((leftShoulderY - leftHandY) <= 20 && (leftShoulderY - leftHandY) >= -20 && leftHandX < leftShoulderX)
            {
                leftArmState = 0;
                gestureCheck = "Left arm straightened";
                if (lWaveSegTwo != true && lWaveSegOne == true)
                {
                    lWaveSegTwo = true;
                }
                else if (lWaveSegThree == true && lWaveSegFour != true)
                {
                    lWaveSegFour = true;
                }
            }
            else if ((leftShoulderY - leftElbowY) <= 20 && (leftShoulderY - leftElbowY) >= -20)
            {
                if ((leftHandX - leftElbowX) <= 20 && (leftHandX - leftElbowX) >= -20)
                {
                    leftArmState = 1;
                    gestureCheck = "Left arm bent half";
                    if (lWaveSegOne != true)
                    {
                        lWaveSegOne = true;
                    }
                    else if (lWaveSegTwo == true && lWaveSegThree != true)
                    {
                        lWaveSegThree = true;
                    }
                }
                else if ((leftShoulderX - leftHandX) <= 20 && (leftShoulderX - leftHandX) >= -20)
                {
                    leftArmState = 2;
                    gestureCheck = "Left arm bent fully";
                }
            }
            else
            {
                leftArmState = -1;
            }
        }
        //Checks if both arms are in certain positions and returns a gesture based on this
        private void handleBothArms(Person person, ref String gestureCheck)
        {
            //Since the arm states are declared as 0 in the straightened arm methods, it simply checks if both of these are true
            if (leftArmState == 0 && rightArmState == 0)
            {
                gestureCheck = "Both arms straight";
                //The same concept as the straightened arms is applied to the other two positions
            }
            else if (leftArmState == 1 && rightArmState == 1)
            {
                gestureCheck = "Both arms half bent";
            }
            else if (leftArmState == 2 && rightArmState == 2)
            {
                gestureCheck = "Both arms bent fully";
            }
        }

        private void handleRightLeg(Person person, ref String gestureCheck)
        {
            int rightFootDepth = person.jFootRight.D;
            int rightHipDepth = person.jHipRight.D;
            //Checks if the depth difference between the hip and the foot is positive to see if the user is kicking towards the camera
            if ((rightHipDepth - rightFootDepth) >= 300)
            {
                gestureCheck = "Kicking right leg";
            }
        }

        private void handleLeftLeg(Person person, ref String gestureCheck)
        {
            int leftFootDepth = person.jFootLeft.D;
            int leftHipDepth = person.jHandLeft.D;
            //Same principle as the right foot kicking
            if ((leftHipDepth - leftFootDepth) >= 300)
            {
                gestureCheck = "Kicking left leg";
            }
        }

        private void handleClap(Person person, ref String gestureCheck)
        {
            int leftHandX = person.jHandLeft.X;
            int leftHandY = person.jHandLeft.Y;
            int rightHandX = person.jHandRight.X;
            int rightHandY = person.jHandRight.Y;
            //Checks if the hands are together to see if the user is clapping
            if ((leftHandX - rightHandX) <= 20 && (leftHandX - rightHandX) >= -20)
            {
                if ((leftHandY - rightHandY) <= 20 && (leftHandY - rightHandY) >= -20)
                {
                    gestureCheck = "Clapping";
                }
            }
        }

        private void handleWave(Person person, ref String gestureCheck)
        {
            //Decrements gesture frames from initial value to 0 
            //If it reaches 0 then the wave segments are reset to begin checking for a wave again
            gestureFrames--;
            if (gestureFrames == 0)
            {
                gestureFrames = 100;
                lWaveSegOne = false;
                lWaveSegTwo = false;
                lWaveSegThree = false;
                lWaveSegFour = false;

                rWaveSegOne = false;
                rWaveSegTwo = false;
                rWaveSegThree = false;
                rWaveSegFour = false;
            }
            //If the final wave segment for either hand is detected then the GUI says which wave has been detected
            if (lWaveSegFour == true)
            {
                gestureCheck = "Left Hand Waving";
            }
            else if (rWaveSegFour == true)
            {
                gestureCheck = "Right Hand Waving";
            }
            else
            {
                //Debug.WriteLine("No wave detected");
            }
        }
        //Handles checking if the user is walking towards, or away from, the camera using the depth values
        private void handleWalk(Person person, ref String gestureCheck)
        {
            //If the starting depth (initialised in Person class) is different enough to the head's position then the user is walking either backwards or forwards
            //If the depth value increases then the user is moving away, thus walking backwards
            if ((person.startingDepth - person.jHead.D) >= 300)
            {
                gestureCheck = "Walking backwards";
                Debug.WriteLine("Gesure: " + gestureCheck);
                person.startingDepth = person.jHead.D;
                //Else if the depth value decreases then the user is moving towards the camera, thus walking forwards
            }
            else if ((person.startingDepth - person.jHead.D) <= -300)
            {
                gestureCheck = "Walking forwards";
                Debug.WriteLine("Gesture: " + gestureCheck);
                person.startingDepth = person.jHead.D;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("The button has been pressed");
        }

        private void button_Dragged(Person person, Button btn)
        {
            int hand_x = person.jHandRight.X;
            int hand_y = person.jHandRight.Y;

            setButtonLocation(btn, hand_x, hand_y);
        }
        private void import_Release(Person person)
        {
            Border currentBorder = importBorder;
            currentDrag = "import";
            any_Release(currentBorder, person);
        }
        private void any_Release(Border currentBorder, Person person)
        {
            double borderLeft = currentBorder.Margin.Left;
            double borderTop = currentBorder.Margin.Top;
            double borderRight = borderLeft + currentBorder.ActualWidth;
            double borderBottom = borderTop + currentBorder.ActualHeight;
            int hand_x = person.jHandRight.X;
            int hand_y = person.jHandRight.Y;

            if (hand_y > borderTop && hand_y < borderBottom && hand_x > borderLeft && hand_x < borderRight && currentDrag == "import")
            {
                Button currentStatement = new Button();
                importBounds.Children.Add(currentStatement);
                Grid.SetRow(currentStatement, importBounds.RowDefinitions.Count);
                currentStatement.Name = "import" + imports.Count;
                imports.Add(currentStatement);
                currentBorder.Height += defaultButtonHeight * 1.8;
                currentStatement.Height = defaultButtonHeight;
                currentStatement.Width = currentBorder.Width;
                currentStatement.Content = "EMPTY";
                double left = borderLeft - borderRight + (currentStatement.Width);
                double top = -1;
                if (imports.Count > 1)
                {
                    top = imports[imports.Count - 2].Margin.Top - 15;
                }
                else
                {
                    top = (importBounds.Height) - importBounds.Height * 1.5;
                }

                setButtonLocation(currentStatement, left, top);
                if (!windowShown)
                {
                    Form1 importExplorer = new Form1(this, currentStatement, "");
                    windowShown = true;
                    hasSelected = false;
                    importExplorer.Show();
                }

                while (!hasSelected)
                {
                    Console.WriteLine("fill out the form");
                }

                foreach (FrameworkElement element in importBounds.Children)
                {
                    //try { }
                    Console.WriteLine(element);
                }

                importBounds.RowDefinitions.Add(new RowDefinition());

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
            }
            defaultStatementLocations();

        }

        public void defaultStatementLocations()
        {
            setButtonLocation(btn_import, imports_x, imports_y);
            setButtonLocation(btn_variable, var_x, var_y);
        }
    }
}
