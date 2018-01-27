using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gesture3
{
    //Class that handles the gesture checks of the kinect skeletons
    class Gesture
    {
        //An instance of the kinect
        Kinect kinect;
        //The interactable elements of the program
        Button activeButton = new Button();
        ComboBox activeBox = new ComboBox();
        ComboBoxItem activeItem = new ComboBoxItem();
        //An instance of the event handler
        public Event events;
        //Checks if the person is pushing
        Boolean pushing = false;
        String prevName = "";

        //Constructor which takes the current instance of the kinect
        public Gesture(Kinect k)
        {
            //Stores the current instance of the kinect
            this.kinect = k;
            //Makes a new instance of the events class
            events = new Event(k);
            //Sets the name of the active combo box as a placeholder
            activeBox.Name = "no1";
        }

        //Method which checks what the current person skeleton is doing
        //Takes the person, as well as the interactable elements from the current page
        public void checkPersonFor(Person person, List<Button> interactable, List<ComboBox> cmbInteract, List<ComboBoxItem> cmbItemInteract)
        {
            //Gets the hand coordinates as a point
            Point hand = getHandXY(person);
            //If the person is not pushing, check what they are hovering over
            if (!pushing)
            {
                isHovering(interactable, hand.X, hand.Y, cmbInteract, cmbItemInteract);
            }
            //Check if the person is pushing
            handlePush(person, hand.X, hand.Y, cmbInteract, cmbItemInteract);

        }

        //Function that returns the coordinates of the active hand
        public Point getHandXY(Person person)
        {
            int x;
            int y;

            //Checks what hand the program is currently tracking and gets its coordinates from the person instance
            if (Settings.rightHandTracked)
            {
                x = person.jHandRight.X;
                y = person.jHandRight.Y;
            }
            else
            {
                x = person.jHandLeft.X;
                y = person.jHandLeft.Y;
            }
            //Make a new point and return both values at once
            return new Point(x, y);
        }

        //Method that checks if the user is hovering over any elements
        public void isHovering(List<Button> btns, double hx, double hy, List<ComboBox> cmbs, List<ComboBoxItem> cmbis)
        {
            //Resets what is currently being hovered
            activeButton = new Button();
            activeItem = new ComboBoxItem();
            activeButton.Name = "no2";
            activeItem.Name = "no3";

            //Check all the comboboxes in the page
            foreach (ComboBox btn in cmbs)
            {
                //Get its location
                double left = Canvas.GetLeft(btn);
                double top = Canvas.GetTop(btn);
                double right = left + btn.Width;
                double bottom = top + btn.Height;
                //If the hand is in that range, set the element as the active combo box
                if (hx > left & hx < right && hy > top && hy < bottom)
                {
                    activeBox = btn;
                }
            }
            if ((activeBox.Name != "no1")) //Stops button interaction while a combobox is open & looks at items
            {
                int count = 1;
                //Goes through each of the items that are available on the page
                foreach (ComboBoxItem btn2 in cmbis)
                {
                    //Save the area
                    double left = Canvas.GetLeft(activeBox);
                    double right = left + activeBox.Width;
                    double top = Canvas.GetTop(activeBox) + (activeBox.Height);
                    double bottom = top + (activeBox.Height * count);
                    //Checks if the hand is in the area and sets it as active
                    if (hx > left & hx < right && hy > top && hy < bottom)
                    {
                        activeItem = btn2;
                        break;
                    }
                }
            }
            else //If there is no active combo box, then look at buttons
            {
                //Boolean value used to decide if the button should change colour on hover or not, used in situtations where button colour indicates something
                Boolean dontColour = false;
                //Go through each button in the page
                foreach (Button btn in btns)
                {
                    //Get the buttons area
                    double left = Canvas.GetLeft(btn);
                    double top = Canvas.GetTop(btn);
                    double right = left + btn.Width;
                    double bottom = top + btn.Height;
                    //Checks if the user's hand is within the button's area
                    if (hx > left & hx < right && hy > top && hy < bottom)
                    {
                        activeButton = btn;
                        //Sets the active button in speech, for use with the 'select' speech gesture
                        kinect.speech.setActiveButton(activeButton);
                        //If the currently hovered button has a differen name to the previous tick, then say it to the user using text to speech
                        if (activeButton.Content.ToString() != prevName)
                        {
                            //Set the previous name to the current name to prevent this happening next tick unless the name changes
                            prevName = activeButton.Content.ToString();
                            //Speak the first word of the button name
                            kinect.frame.textToSpeech(activeButton.Content.ToString().Split(' ')[0]);
                        }

                        //Check if the active window is either inputs or settings as they are the only ones where button colour can matter
                        if (kinect.currentWindow == "input" || kinect.currentWindow == "settings")
                        {
                            //Checks the current button to see if it should retain its colour when set
                            //This only applies to the input and settings pages, since settings has options that must retain their colour and input has the operator and indent options
                            switch (activeButton.Content.ToString())
                            {
                                case "Indent":
                                case "+":
                                case "-":
                                case "/":
                                case "*":
                                case "==":
                                case ">=":
                                case ">":
                                case "<=":
                                case "<":
                                case "Left":
                                case "Right":
                                case "Large":
                                case "Normal":
                                    //If any of these buttons are being hovered, set a flag to stop them changing colour later
                                    dontColour = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //If dontColour is set to true by the button being one of the ones listed in the case break out of the loop
                    if (dontColour)
                    {
                        return;
                    }
                    //Set any other button than these to a black background and white text which is the non-hovered colours
                    switch (btn.Content.ToString())
                    {
                        case "Indent":
                        case "+":
                        case "-":
                        case "/":
                        case "*":
                        case "==":
                        case ">=":
                        case ">":
                        case "<=":
                        case "<":
                        case "Left":
                        case "Right":
                        case "Large":
                        case "Normal":
                            break;

                        default:
                            btn.Background = Brushes.Black;
                            btn.Foreground = Brushes.White;
                            break;
                    }

                }
            }

            //Sets the background colour of any active items to show they are hovered
            if (activeButton.Name != "no2")
            {
                activeButton.Background = Brushes.White;
                activeButton.Foreground = Brushes.Black;
            }
            else if (activeBox.Name != "no1")
            {
                activeBox.Background = Brushes.White;
                activeBox.Foreground = Brushes.Black;
            }
            else if (activeItem.Name != "no3")
            {
                Console.WriteLine("item: " + activeItem.Content);
                activeItem.Background = Brushes.White;
                activeItem.Foreground = Brushes.Black;
            }
        }

        //Method to check if the person is pushing over an interactive element
        public void handlePush(Person person, double x, double y, List<ComboBox> cmbInteract, List<ComboBoxItem> cmbItemInteract)
        {
            double difference;
            //Gets the difference between the active hand and the spine depth
            if (Settings.rightHandTracked)
            {
                difference = person.jSpine.D - person.jHandRight.D;
            }
            else
            {
                difference = person.jSpine.D - person.jHandLeft.D;
            }

            //Checks if the difference is more than the threshold that set in the person class
            if (difference > person.pushDif)
            {
                //If so, the person is doing a pushing gesture
                pushing = true;
                //Checks if there are any active elements from the hover method
                if (activeButton.Name != "no2")
                {
                    //If the buttons name is in this list, trigger the dragging event in the events class
                    switch (activeButton.Content.ToString())
                    {
                        case "Variable (4)":
                        case "If (1)":
                        case "Print (2)":
                        case "Expression (5)":
                        case "Loop (3)":
                            events.buttonDragged(x, y, activeButton);
                            break;
                    }
                }
                //If a push is performed on an active combo box item, close the combo box and set the item as selected
                if (activeItem.Name != "no3")
                {
                    activeBox.SelectedItem = activeItem;
                    activeBox.IsDropDownOpen = false;
                    //Create a new instance to allow for button presses again
                    activeBox = new ComboBox();
                    activeBox.Name = "no1";
                }
            }
            else //If the user is not pushing...
            {
                if (pushing) //If the user has an unhandled release after a push...
                {
                    //Allow for the hovering method to tick again
                    pushing = false;
                    //Check what the active element is
                    if (activeButton.Name != "no2")
                    {
                        switch (activeButton.Content.ToString())
                        {
                            //If the button is any of these, then trigger the release method to make a new statement
                            case "Variable (4)":
                            case "If (1)":
                            case "Print (2)":
                            case "Expression (5)":
                            case "Loop (3)":
                                //If a statement, do this
                                events.buttonReleased(x, y, activeButton);
                                break;
                            default:
                                //If not a statement, its a click event
                                Console.WriteLine("Not a drag");
                                events.clickHandler(activeButton);
                                break;
                        }
                    }
                    else if (activeBox.Name != "no1")
                    {
                        //Open or close the drop down menu
                        if (activeBox.IsDropDownOpen == false)
                        {
                            activeBox.IsDropDownOpen = true;
                        }
                        else
                        {
                            //Close the drop down menu and make a new instance of the active combo box
                            activeBox.IsDropDownOpen = false;
                            activeBox = new ComboBox();
                            activeBox.Name = "no1";
                        }
                    }
                }
            }
        }

        //Gets a list of all the variable names in the code window
        public List<String> getVariables()
        {
            List<String> temp = events.getVariables();
            return temp;
        }
    }
}
